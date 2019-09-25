using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Czeum.DAL;
using Czeum.DAL.Entities;
using Czeum.DTO.UserManagement;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Application.Services.FriendService
{
    public class FriendService : IFriendService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public FriendService(ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<string>> GetFriendsOfUserAsync(string user)
        {
            return await context.Friendships
                .Where(f => f.User1.UserName == user || f.User2.UserName == user)
                .Select(f => f.User1.UserName == user ? f.User2.UserName : f.User1.UserName)
                .ToListAsync();
        }

        public async Task AcceptRequestAsync(Guid requestId)
        {
            var request = await context.Requests.Include(r => r.Sender)
                .Include(r => r.Receiver)
                .SingleAsync(r => r.Id == requestId);

            var friendship = new Friendship
            {
                User1 = request.Sender,
                User2 = request.Receiver
            };

            context.Friendships.Add(friendship);
            context.Requests.Remove(request);
            await context.SaveChangesAsync();
        }

        public async Task RemoveFriendAsync(Guid friendshipId)
        {
            var friendship = await context.Friendships.FindAsync(friendshipId);

            context.Friendships.Remove(friendship);
            await context.SaveChangesAsync();
        }

        public async Task<FriendRequestDto> AddRequestAsync(string sender, string receiver)
        {
            var alreadyRequestedOrFriends = await context.Users.Where(u => u.UserName == sender)
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
                Sender = await context.Users.SingleAsync(u => u.UserName == sender),
                Receiver = await context.Users.SingleAsync(u => u.UserName == receiver)
            };

            context.Requests.Add(request);
            await context.SaveChangesAsync();

            return mapper.Map<FriendRequestDto>(request);
        }

        public async Task RemoveRequestAsync(Guid requestId)
        {
            var request = await context.Requests.FindAsync(requestId);

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