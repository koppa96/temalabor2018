using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.DAL;
using Czeum.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Application.Services.FriendService
{
    public class FriendService : IFriendService
    {
        private readonly ApplicationDbContext context;

        public FriendService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<string>> GetFriendsOfUserAsync(string user)
        {
            return await context.Friendships
                .Where(f => f.User1.UserName == user || f.User2.UserName == user)
                .Select(f => f.User1.UserName == user ? f.User2.UserName : f.User1.UserName)
                .ToListAsync();
        }

        public async Task AcceptRequestAsync(string sender, string receiver)
        {
            var request = await context.Requests
                .SingleAsync(r => r.Sender.UserName == sender && r.Receiver.UserName == receiver);

            var friendship = new Friendship
            {
                User1 = request.Sender,
                User2 = request.Receiver
            };

            context.Friendships.Add(friendship);
            context.Requests.Remove(request);
            await context.SaveChangesAsync();
        }

        public async Task RemoveFriendAsync(string user, string friend)
        {
            var friendship = await context.Friendships
                .SingleAsync(f => f.User1.UserName == user && f.User2.UserName == friend ||
                                  f.User2.UserName == user && f.User1.UserName == friend);

            context.Friendships.Remove(friendship);
            await context.SaveChangesAsync();
        }

        public async Task AddRequestAsync(string sender, string receiver)
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
        }

        public async Task RemoveRequestAsync(string sender, string receiver)
        {
            var request = await context.Requests
                .SingleAsync(r => r.Sender.UserName == sender && r.Receiver.UserName == receiver);

            context.Requests.Remove(request);
            await context.SaveChangesAsync();
        }

        public async Task<List<string>> GetRequestsSentByUserAsync(string user)
        {
            return await context.Requests.Where(r => r.Sender.UserName == user)
                .Select(r => r.Receiver.UserName)
                .ToListAsync();
        }

        public async Task<List<string>> GetRequestsReceivedByUserAsync(string user)
        {
            return await context.Requests.Where(r => r.Receiver.UserName == user)
                .Select(r => r.Sender.UserName)
                .ToListAsync();
        }
    }
}