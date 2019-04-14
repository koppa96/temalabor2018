using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.DAL;
using Czeum.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Server.Services.FriendService
{
    public class FriendService : IFriendService
    {
        private readonly IApplicationDbContext _context;

        public FriendService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetFriendsOfUserAsync(string user)
        {
            return await _context.Friendships
                .Where(f => f.User1.UserName == user || f.User2.UserName == user)
                .Select(f => f.User1.UserName == user ? f.User2.UserName : f.User1.UserName)
                .ToListAsync();
        }

        public async Task AcceptRequestAsync(string sender, string receiver)
        {
            var request = await _context.Requests
                .SingleAsync(r => r.Sender.UserName == sender && r.Receiver.UserName == receiver);

            var friendship = new Friendship
            {
                User1 = request.Sender,
                User2 = request.Receiver
            };

            _context.Friendships.Add(friendship);
            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFriendAsync(string user, string friend)
        {
            var friendship = await _context.Friendships
                .SingleAsync(f => f.User1.UserName == user && f.User2.UserName == friend ||
                                  f.User2.UserName == user && f.User1.UserName == friend);

            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();
        }

        public async Task AddRequestAsync(string sender, string receiver)
        {
            var alreadyRequestedOrFriends = await _context.Users.Where(u => u.UserName == sender)
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
                Sender = await _context.Users.SingleAsync(u => u.UserName == sender),
                Receiver = await _context.Users.SingleAsync(u => u.UserName == receiver)
            };

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRequestAsync(string sender, string receiver)
        {
            var request = await _context.Requests
                .SingleAsync(r => r.Sender.UserName == sender && r.Receiver.UserName == receiver);

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetRequestsSentByUserAsync(string user)
        {
            return await _context.Requests.Where(r => r.Sender.UserName == user)
                .Select(r => r.Receiver.UserName)
                .ToListAsync();
        }

        public async Task<List<string>> GetRequestsReceivedByUserAsync(string user)
        {
            return await _context.Requests.Where(r => r.Receiver.UserName == user)
                .Select(r => r.Sender.UserName)
                .ToListAsync();
        }
    }
}