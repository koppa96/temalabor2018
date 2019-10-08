using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Czeum.Core.DTOs.UserManagement;
using Czeum.Core.Services;
using Czeum.DAL;
using Czeum.DAL.Extensions;
using Czeum.Domain.Entities;
using Czeum.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Application.Services
{
    public class FriendService : IFriendService
    {
        private readonly CzeumContext context;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly IOnlineUserTracker onlineUserTracker;
        private readonly INotificationService notificationService;

        public FriendService(CzeumContext context,
            IMapper mapper,
            IIdentityService identityService,
            IOnlineUserTracker onlineUserTracker,
            INotificationService notificationService)
        {
            this.context = context;
            this.mapper = mapper;
            this.identityService = identityService;
            this.onlineUserTracker = onlineUserTracker;
            this.notificationService = notificationService;
        }

        public async Task<IEnumerable<FriendDto>> GetFriendsOfUserAsync(string user)
        {
            return (await context.Friendships.Include(f => f.User1)
                .Include(f => f.User2)
                .Where(f => f.User1.UserName == user || f.User2.UserName == user)
                .ToListAsync())
                .Select(f =>
                {
                    var friendName = f.User1.UserName == user ? f.User2.UserName : f.User1.UserName;
                    return new FriendDto
                    {
                        FriendshipId = f.Id,
                        IsOnline = onlineUserTracker.IsOnline(friendName),
                        Username = friendName
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

        public async Task<FriendRequestDto> AddRequestAsync(string receiver)
        {
            var currentUser = identityService.GetCurrentUserName();
            var alreadyRequestedOrFriends = await context.Users.Where(u => u.UserName == currentUser)
                .AnyAsync(u => u.SentRequests.Any(r => r.Receiver.UserName == receiver) ||
                               u.ReceivedRequests.Any(r => r.Sender.UserName == receiver) ||
                               u.User1Friendships.Any(f => f.User2.UserName == receiver) ||
                               u.User2Friendships.Any(f => f.User1.UserName == receiver));

            if (alreadyRequestedOrFriends)
            {
                throw new InvalidOperationException("There is already a request or friendship between there users.");
            }
            
            var request = new FriendRequest
            {
                Sender = await context.Users.SingleAsync(u => u.UserName == currentUser),
                Receiver = await context.Users.CustomSingleAsync(u => u.UserName == receiver, 
                    "No user with the given name exists.")
            };

            context.Requests.Add(request);
            await context.SaveChangesAsync();
            var requestDto = mapper.Map<FriendRequestDto>(request);

            await notificationService.NotifyAsync(
                request.Receiver.UserName, client => client.ReceiveRequest(requestDto));

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
    }
}