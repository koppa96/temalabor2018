using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Czeum.Application.Extensions;
using Czeum.Application.Services.Lobby;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Paging;
using Czeum.Core.Services;
using Czeum.DAL;
using Czeum.DAL.Extensions;
using Czeum.Domain.Entities;
using Czeum.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace Czeum.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly CzeumContext context;
        private readonly ILobbyStorage lobbyStorage;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly INotificationService notificationService;

        public MessageService(
            CzeumContext context, 
            ILobbyStorage lobbyStorage, 
            IMapper mapper,
            IIdentityService identityService,
            INotificationService notificationService)
        {
            this.context = context;
            this.lobbyStorage = lobbyStorage;
            this.mapper = mapper;
            this.identityService = identityService;
            this.notificationService = notificationService;
        }

        public async Task<Message> SendToLobbyAsync(Guid lobbyId, string message)
        {
            var sender = identityService.GetCurrentUserName();
            var lobby = lobbyStorage.GetLobby(lobbyId);
            if (lobby.Host != sender && !lobby.Guests.Contains(sender))
            {
                throw new UnauthorizedAccessException("Not authorized to send message to this lobby.");
            }

            var msg = new Message
            {
                Sender = sender,
                Text = message,
                Timestamp = DateTime.UtcNow
            };
            lobbyStorage.AddMessage(lobbyId, msg);
            await notificationService.NotifyAsync(lobby.Others(sender), 
                client => client.ReceiveLobbyMessage(lobbyId, msg));
            return msg;
        }

        public async Task<Message> SendToMatchAsync(Guid matchId, string message)
        {
            var senderId = identityService.GetCurrentUserId();
            var match = await context.Matches.Include(m => m.Users)
                    .ThenInclude(um => um.User)
                .CustomSingleAsync(m => m.Id == matchId);
            
            if (match.Users.All(um => um.UserId != senderId))
            {
                throw new UnauthorizedAccessException("Not authorized to send message to this match.");
            }

            var senderUser = await context.Users.FindAsync(senderId);
            var storedMessage = new StoredMessage
            {
                Sender = senderUser,
                Match = match,
                Text = message,
                Timestamp = DateTime.UtcNow
            };
            context.MatchMessages.Add(storedMessage);
            await context.SaveChangesAsync();
            
            var sentMessage = mapper.Map<Message>(storedMessage);
            await notificationService.NotifyAsync(match.Others(identityService.GetCurrentUserName()),
                client => client.ReceiveMatchMessage(match.Id, sentMessage));

            return sentMessage;
        }

        public Task<RollListDto<Message>> GetMessagesOfLobbyAsync(Guid lobbyId, Guid? oldestId,
            int requestedCount)
        {
            var lobby = lobbyStorage.GetLobby(lobbyId);
            if (!lobby.Contains(identityService.GetCurrentUserName()))
            {
                throw new UnauthorizedAccessException("Not authorized to read the messages of this lobby.");
            }

            List<Message>? results = null;
            if (oldestId.HasValue)
            {
                var oldest = lobbyStorage.GetMessages(lobbyId).Single(x => x.Id == oldestId);
                results = lobbyStorage.GetMessages(lobbyId)
                    .Where(x => x.Timestamp > oldest.Timestamp)
                    .OrderByDescending(x => x.Timestamp)
                    .Take(requestedCount)
                    .OrderBy(x => x.Timestamp)
                    .ToList();
            }
            else
            {
                results = lobbyStorage.GetMessages(lobbyId)
                    .OrderByDescending(x => x.Timestamp)
                    .Take(requestedCount)
                    .OrderBy(x => x.Timestamp)
                    .ToList();
            }
            
            var hasMore = results.Count < requestedCount || lobbyStorage.GetMessages(lobbyId)
                          .Any(x => x.Timestamp > results.Last().Timestamp);
            
            return Task.FromResult(new RollListDto<Message>
            {
                HasMoreLeft = hasMore,
                Data = results
            });
        }

        public async Task<RollListDto<Message>> GetMessagesOfMatchAsync(Guid matchId, Guid? oldestId,
            int requestedCount)
        {
            var currentUserId = identityService.GetCurrentUserId();

            var match = await context.Matches.Include(m => m.Users)
                .Include(x => x.Messages)
                    .ThenInclude(x => x.Sender)
                .CustomSingleAsync(m => m.Id == matchId, "No match with the given id was found.");

            if (match.Users.All(um => um.UserId != currentUserId))
            {
                throw new UnauthorizedAccessException("Not authorized to read the messages of this lobby.");
            }

            List<StoredMessage>? results = null;
            if (oldestId.HasValue)
            {
                var oldest = match.Messages.Single(x => x.Id == oldestId);
                results = match.Messages.Where(x => x.Timestamp > oldest.Timestamp)
                    .OrderByDescending(x => x.Timestamp)
                    .Take(requestedCount)
                    .OrderBy(x => x.Timestamp)
                    .ToList();
            }
            else
            {
                results = match.Messages.OrderByDescending(x => x.Timestamp)
                    .Take(requestedCount)
                    .OrderBy(x => x.Timestamp)
                    .ToList();
            }

            var hasMore = results.Count < requestedCount || 
                match.Messages.Any(x => x.Timestamp > results.Last().Timestamp);

            return new RollListDto<Message>
            {
                HasMoreLeft = hasMore,
                Data = mapper.Map<List<Message>>(results)
            };
        }

        public async Task<Message> SendToFriendAsync(Guid friendshipId, string message)
        {
            var senderId = identityService.GetCurrentUserId();
            var friendship = await context.Friendships
                .Include(x => x.User1)
                .Include(x => x.User2)
                .CustomSingleAsync(f => f.Id == friendshipId);

            if (friendship.User1Id != senderId && friendship.User2Id != senderId)
            {
                throw new UnauthorizedAccessException("You are not a member of this friendship.");
            }

            var directMessage = new DirectMessage
            {
                Sender = senderId == friendship.User1Id ? friendship.User1 : friendship.User2,
                Friendship = friendship,
                Text = message,
                Timestamp = DateTime.UtcNow
            };
            context.DirectMessages.Add(directMessage);
            await context.SaveChangesAsync();

            var sentMessage = mapper.Map<Message>(directMessage);
            await notificationService.NotifyAsync(friendship.User1Id == senderId ? friendship.User2.UserName : friendship.User1.UserName,
                client => client.ReceiveDirectMessage(friendship.Id, sentMessage));

            return sentMessage;
        }

        public async Task<RollListDto<Message>> GetMessagesOfFrienshipAsync(Guid friendshipId, Guid? oldestId, int requestedCount)
        {
            var currentUserId = identityService.GetCurrentUserId();

            var friendship = await context.Friendships.Include(x => x.Messages)
                    .ThenInclude(x => x.Sender)
                .CustomSingleAsync(x => x.Id == friendshipId, "No match with the given id was found");

            if (friendship.User1Id != currentUserId && friendship.User2Id != currentUserId)
            {
                throw new UnauthorizedAccessException("You are not a member of this friendship.");
            }

            List<DirectMessage>? results = null;
            if (oldestId.HasValue)
            {
                var oldest = friendship.Messages.Single(x => x.Id == oldestId);
                results = friendship.Messages.Where(x => x.Timestamp > oldest.Timestamp)
                    .OrderByDescending(x => x.Timestamp)
                    .Take(requestedCount)
                    .OrderBy(x => x.Timestamp)
                    .ToList();
            }
            else
            {
                results = friendship.Messages.OrderByDescending(x => x.Timestamp)
                    .Take(requestedCount)
                    .OrderBy(x => x.Timestamp)
                    .ToList();
            }

            var hasMore = results.Count < requestedCount || friendship.Messages
                .Any(x => x.Timestamp > results.Last().Timestamp);

            return new RollListDto<Message>
            {
                HasMoreLeft = hasMore,
                Data = mapper.Map<List<Message>>(results)
            };
        }
    }
}