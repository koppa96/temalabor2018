using System;
using System.Collections.Generic;
using System.Linq;
using Czeum.DAL.Entities;
using Czeum.DAL.Interfaces;

namespace Czeum.DAL.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        private readonly ApplicationDbContext _context;
        
        public FriendRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public List<string> GetFriendsOf(string user)
        {
            return _context.Friendships
                .Where(f => f.User1.UserName == user || f.User2.UserName == user)
                .Select(f => f.User1.UserName == user ? f.User2.UserName : f.User1.UserName)
                .ToList();
        }

        public void AcceptRequest(string sender, string receiver)
        {
            var request = _context.Requests.SingleOrDefault(r => r.Sender.UserName == sender && r.Receiver.UserName == receiver);
            if (request == null)
            {
                throw new ArgumentException($"There is no request sent by {sender} to {receiver}");
            }

            var friendship = new Friendship
            {
                User1 = request.Sender,
                User2 = request.Receiver
            };

            _context.Friendships.Add(friendship);
            _context.Requests.Remove(request);
        }

        private Friendship GetFriendshipByFriendNames(string user, string friend)
        {
            return _context.Friendships.SingleOrDefault(f =>
                f.User1.UserName == user && f.User2.UserName == friend ||
                f.User2.UserName == user && f.User1.UserName == friend);
        }

        private FriendRequest GetRequestByNames(string sender, string receiver)
        {
           return _context.Requests.SingleOrDefault(r =>
                r.Sender.UserName == sender && r.Receiver.UserName == receiver ||
                r.Sender.UserName == receiver && r.Receiver.UserName == sender);
        }

        public void RemoveFriend(string user, string friend)
        {
            var friendship = GetFriendshipByFriendNames(user, friend);
            if (friendship == null)
            {
                throw new ArgumentException($"There is no friendship between {user} and {friend}");
            }

            _context.Friendships.Remove(friendship);
        }

        public void AddRequest(string sender, string receiver)
        {
            var request = GetRequestByNames(sender, receiver);
            if (request != null)
            {
                throw new ArgumentException($"There is already a friend request between {sender} and {receiver}.");
            }

            var friendship = GetFriendshipByFriendNames(sender, receiver);
            if (friendship != null)
            {
                throw new ArgumentException($"There is already a friendship between {sender} and {receiver}.");
            }

            request = new FriendRequest
            {
                Sender = _context.Users.SingleOrDefault(u => u.UserName == sender),
                Receiver = _context.Users.SingleOrDefault(u => u.UserName == receiver)
            };

            if (request.Sender == null)
            {
                throw new ArgumentOutOfRangeException(nameof(sender), "There is no such user.");
            }

            if (request.Receiver == null)
            {
                throw new ArgumentOutOfRangeException(nameof(receiver), "There is no such user.");
            }

            _context.Requests.Add(request);
        }

        public void RemoveRequest(string sender, string receiver)
        {
            var request = GetRequestByNames(sender, receiver);
            if (request == null)
            {
                throw new ArgumentException("There is no such request.");
            }

            _context.Requests.Remove(request);
        }

        public List<string> GetRequestsSentBy(string user)
        {
            return _context.Requests.Where(r => r.Sender.UserName == user)
                .Select(r => r.Receiver.UserName)
                .ToList();
        }

        public List<string> GetRequestsReceivedBy(string user)
        {
            return _context.Requests.Where(r => r.Receiver.UserName == user)
                .Select(r => r.Sender.UserName)
                .ToList();
        }
    }
}