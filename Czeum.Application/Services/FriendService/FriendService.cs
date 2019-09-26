using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Czeum.Application.Services.OnlineUsers;
using Czeum.DAL;
using Czeum.DAL.Entities;
using Czeum.DAL.Extensions;
using Czeum.DTO.UserManagement;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Application.Services.FriendService
{
    public class FriendService : IFriendService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly IOnlineUserTracker onlineUserTracker;

        public FriendService(ApplicationDbContext context,
            IMapper mapper,
            IIdentityService identityService,
            IOnlineUserTracker onlineUserTracker)
        {
            this.context = context;
            this.mapper = mapper;
            this.identityService = identityService;
            this.onlineUserTracker = onlineUserTracker;
        }

        public async Task<List<string>> GetFriendsOfUserAsync(string user)
        {
            return await context.Friendships
                .Where(f => f.User1.UserName == user || f.User2.UserName == user)
                .Select(f => f.User1.UserName == user ? f.User2.UserName : f.User1.UserName)
                .ToListAsync();
        }

        public async Task<(FriendDto Sender, FriendDto Receiver)> AcceptRequestAsync(Guid requestId)
        {
            var currentUser = identityService.GetCurrentUser();
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

            return (
                Sender: new FriendDto
                {
                    FriendshipId = friendship.Id,
                    IsOnline = true,
                    Username = friendship.User2.UserName
                }, 
                Receiver: new FriendDto
                {
                    FriendshipId = friendship.Id,
                    IsOnline = onlineUserTracker.IsOnline(friendship.User1.UserName),
                    Username = friendship.User1.UserName
                }
            );
        }

        public async Task RemoveFriendAsync(Guid friendshipId)
        {
            var currentUser = identityService.GetCurrentUser();
            var friendship = await context.Friendships.Include(f => f.User1)
                .Include(f => f.User2)
                .CustomSingleAsync(f => f.Id == friendshipId, "No friendship found with the given id.");

            if (currentUser != friendship.User1.UserName && currentUser != friendship.User2.UserName)
            {
                throw new UnauthorizedAccessException("You can not delete a friendship that you are not part of.");
            }

            context.Friendships.Remove(friendship);
            await context.SaveChangesAsync();
        }

        public async Task<FriendRequestDto> AddRequestAsync(string receiver)
        {
            var currentUser = identityService.GetCurrentUser();
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

            return mapper.Map<FriendRequestDto>(request);
        }

        public async Task RemoveRequestAsync(Guid requestId)
        {
            var request = await context.Requests.CustomFindAsync(requestId, 
                "No friend request found with the given id.");

            context.Requests.Remove(request);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FriendRequestDto>> GetRequestsSentByUserAsync(string user)
        {
            return (await context.Requests.Include(r => r.Sender)
                .Include(r => r.Receiver)
                .Where(r => r.Sender.UserName == user)
                .ToListAsync())
                .Select(r => mapper.Map<FriendRequestDto>(r));
        }

        public async Task<IEnumerable<FriendRequestDto>> GetRequestsReceivedByUserAsync(string user)
        {
            return (await context.Requests.Include(r => r.Sender)
                .Include(r => r.Receiver)
                .Where(r => r.Receiver.UserName == user)
                .ToListAsync())
                .Select(r => mapper.Map<FriendRequestDto>(r));
        }
    }
}