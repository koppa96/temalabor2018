using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Czeum.Application.Extensions;
using Czeum.Application.Services.Lobby;
using Czeum.DAL;
using Czeum.DAL.Extensions;
using Czeum.Domain.Entities;
using Czeum.Domain.Services;
using Czeum.DTO;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Application.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly CzeumContext context;
        private readonly ILobbyStorage lobbyStorage;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;

        public MessageService(
            CzeumContext context, 
            ILobbyStorage lobbyStorage, 
            IMapper mapper,
            IIdentityService identityService)
        {
            this.context = context;
            this.lobbyStorage = lobbyStorage;
            this.mapper = mapper;
            this.identityService = identityService;
        }

        public Message SendToLobby(Guid lobbyId, string message)
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
            return msg;
        }

        public async Task<Message> SendToMatchAsync(Guid matchId, string message)
        {
            var senderId = identityService.GetCurrentUserId();
            var match = await context.Matches.Include(m => m.Users)
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
            context.Messages.Add(storedMessage);
            await context.SaveChangesAsync();
            
            return mapper.Map<Message>(storedMessage);
        }

        public IEnumerable<Message> GetMessagesOfLobby(Guid lobbyId)
        {
            var lobby = lobbyStorage.GetLobby(lobbyId);
            if (!lobby.Contains(identityService.GetCurrentUserName()))
            {
                throw new UnauthorizedAccessException("Not authorized to read the messages of this lobby.");
            }

            return lobbyStorage.GetMessages(lobbyId);
        }

        public async Task<IEnumerable<Message>> GetMessagesOfMatchAsync(Guid matchId)
        {
            var currentUserId = identityService.GetCurrentUserId();

            var match = await context.Matches.Include(m => m.Users)
                .Include(m => m.Messages)
                .CustomSingleAsync(m => m.Id == matchId, "No match with the given id was found.");

            if (match.Users.All(um => um.UserId != currentUserId))
            {
                throw new UnauthorizedAccessException("Not authorized to read the messages of this lobby.");
            }

            return match.Messages.Select(mapper.Map<Message>);
        }
    }
}