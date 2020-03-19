using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Mappers;
using Czeum.Core.DTOs.Notifications;
using Czeum.Core.DTOs.UserManagement;
using Czeum.Core.Exceptions;
using Czeum.Core.Services;
using Czeum.DAL;
using Czeum.DAL.Extensions;
using Czeum.Domain.Entities;
using Czeum.Domain.Services;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;

namespace Czeum.Application.Services
{
    public class FriendService : IFriendService
    {
        private readonly CzeumContext context;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly IOnlineUserTracker onlineUserTracker;
        private readonly INotificationService notificationService;
        private readonly INotificationPersistenceService notificationPersistenceService;

        public FriendService(CzeumContext context,
            IMapper mapper,
            IIdentityService identityService,
            IOnlineUserTracker onlineUserTracker,
            INotificationService notificationService,
            INotificationPersistenceService notificationPersistenceService)
        {
            this.context = context;
            this.mapper = mapper;
            this.identityService = identityService;
            this.onlineUserTracker = onlineUserTracker;
            this.notificationService = notificationService;
            this.notificationPersistenceService = notificationPersistenceService;
        }

        public async Task<IEnumerable<FriendDto>> GetFriendsOfUserAsync(string user)
        {
            return (await context.Friendships.Include(f => f.User1)
                .Include(f => f.User2)
                .Where(f => f.User1.UserName == user || f.User2.UserName == user)
                .ToListAsync())
                .Select(f =>
                {
                    var friend = f.User1.UserName == user ? f.User2 : f.User1;
                    return new FriendDto
                    {
                        FriendshipId = f.Id,
                        IsOnline = onlineUserTracker.IsOnline(friend.UserName),
                        Username = friend.UserName,
                        LastDisconnect = friend.LastDisconnected,
                        RegistrationTime = friend.CreatedAt
                    };
                });
        }

        public async Task<FriendDto> AcceptRequestAsync(Guid requestId)
        {
            var currentUser = identityService.GetCurrentUserName();
            var request = await context.Requests.Include(r => r.Sender)
                .Include(r => r.Receiver)
                .CustomSingleAsync(r => r.Id == requestId, "No friend request found with the given id.");

            if (request.Receiver.UserName != currentUser)
            {
                throw new UnauthorizedAccessException("You can not accept someone else's friend request.");
            }

            var friendship = new Friendship
            {
                User1 = request.Sender,
                User2 = request.Receiver
            };

            context.Friendships.Add(friendship);
            context.Requests.Remove(request);
            await context.SaveChangesAsync();

            await notificationService.NotifyAsync(friendship.User1.UserName,
                client => client.FriendAdded(new FriendDto
                {
                    FriendshipId = friendship.Id,
                    IsOnline = onlineUserTracker.IsOnline(friendship.User2.UserName),
                    Username = friendship.User2.UserName
                }));

            await notificationPersistenceService.PersistNotificationAsync(NotificationType.FriendRequestAccepted,
                request.Receiver.Id,
                request.Sender.Id);
            
            return new FriendDto
            {
                FriendshipId = friendship.Id,
                IsOnline = onlineUserTracker.IsOnline(friendship.User1.UserName),
                Username = friendship.User1.UserName
            };
        }

        public async Task RemoveFriendAsync(Guid friendshipId)
        {
            var currentUser = identityService.GetCurrentUserName();
            var friendship = await context.Friendships.Include(f => f.User1)
                .Include(f => f.User2)
                .CustomSingleAsync(f => f.Id == friendshipId, "No friendship found with the given id.");

            if (currentUser != friendship.User1.UserName && currentUser != friendship.User2.UserName)
            {
                throw new UnauthorizedAccessException("You can not delete a friendship that you are not part of.");
            }

            context.Friendships.Remove(friendship);
            await context.SaveChangesAsync();
            await notificationService.NotifyAsync(
                friendship.User1.UserName == currentUser ? friendship.User2.UserName : friendship.User1.UserName,
                client => client.FriendRemoved(friendshipId));
        }

        public async Task<FriendRequestDto> AddRequestAsync(Guid receiverId)
        {
            var currentUser = identityService.GetCurrentUserName();
            var alreadyRequestedOrFriends = await context.Users.Where(u => u.UserName == currentUser)
                .AnyAsync(u => u.SentRequests.Any(r => r.ReceiverId == receiverId) ||
                               u.ReceivedRequests.Any(r => r.SenderId == receiverId) ||
                               u.User1Friendships.Any(f => f.User2Id == receiverId) ||
                               u.User2Friendships.Any(f => f.User1Id == receiverId));

            if (alreadyRequestedOrFriends)
            {
                throw new InvalidOperationException("There is already a request or friendship between there users.");
            }
            
            var request = new FriendRequest
            {
                Sender = await context.Users.SingleAsync(u => u.UserName == currentUser),
                Receiver = await context.Users.CustomFindAsync(receiverId, "No user with the given name exists.")
            };

            context.Requests.Add(request);
            await context.SaveChangesAsync();
            var requestDto = mapper.Map<FriendRequestDto>(request);

            await notificationService.NotifyAsync(
                request.Receiver.UserName, client => client.ReceiveRequest(requestDto));

            await notificationPersistenceService.PersistNotificationAsync(NotificationType.FriendRequestReceived,
                request.Receiver.Id,
                request.Sender.Id,
                request.Id);

            return requestDto;
        }

        public async Task RejectRequestAsync(Guid requestId)
        {
            var currentUserId = identityService.GetCurrentUserId();
            var request = await context.Requests.Include(r => r.Sender)
                .CustomSingleAsync(r => r.Id == requestId, "No request with the given id was found.");

            if (request.ReceiverId != currentUserId)
            {
                throw new UnauthorizedAccessException("A request can only be rejected by its receiver.");
            }

            context.Requests.Remove(request);
            await context.SaveChangesAsync();
            await notificationService.NotifyAsync(request.Sender.UserName,
                client => client.RequestRejected(requestId));
        }

        public async Task RevokeRequestAsync(Guid requestId)
        {
            var currentUserId = identityService.GetCurrentUserId();
            var request = await context.Requests.Include(r => r.Receiver)
                .CustomSingleAsync(r => r.Id == requestId, "No request with the given id was found.");

            if (request.SenderId != currentUserId)
            {
                throw new UnauthorizedAccessException("A request can only be revoked by its sender.");
            }

            context.Requests.Remove(request);
            await context.SaveChangesAsync();
            await notificationService.NotifyAsync(request.Receiver.UserName,
                client => client.RequestRevoked(requestId));
        }

        public async Task<IEnumerable<FriendRequestDto>> GetRequestsSentAsync()
        {
            var currentUser = identityService.GetCurrentUserName();

            return (await context.Requests.Include(r => r.Sender)
                .Include(r => r.Receiver)
                .Where(r => r.Sender.UserName == currentUser)
                .ToListAsync())
                .Select(r => mapper.Map<FriendRequestDto>(r));
        }

        public async Task<IEnumerable<FriendRequestDto>> GetRequestsReceivedAsync()
        {
            var currentUser = identityService.GetCurrentUserName();

            return (await context.Requests.Include(r => r.Sender)
                .Include(r => r.Receiver)
                .Where(r => r.Receiver.UserName == currentUser)
                .ToListAsync())
                .Select(r => mapper.Map<FriendRequestDto>(r));
        }

        public async Task<Dictionary<string, FriendDto>> GetNotificationDataAsync(string username)
        {
            var friendships = await context.Friendships
                .Include(x => x.User1)
                .Include(x => x.User2)
                .Where(x => x.User1.UserName == username || x.User2.UserName == username)
                .ToListAsync();

            return friendships.Select(x =>
            {
                var currentUser = x.User1.UserName == username ? x.User1 : x.User2;
                var friend = x.User1.UserName == username ? x.User2 : x.User1;

                return new
                {
                    FriendName = friend.UserName,
                    Data = new FriendDto
                    {
                        FriendshipId = x.Id,
                        Username = currentUser.UserName,
                        IsOnline = onlineUserTracker.IsOnline(currentUser.UserName),
                        LastDisconnect = currentUser.LastDisconnected,
                        RegistrationTime = currentUser.CreatedAt
                    }
                };
            }).ToDictionary(x => x.FriendName, x => x.Data);
        }
    }
}