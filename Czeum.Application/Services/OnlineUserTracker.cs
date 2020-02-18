using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Czeum.Core.Services;

namespace Czeum.Application.Services
{
    public class OnlineUserTracker : IOnlineUserTracker
    {
        private readonly ConcurrentDictionary<string, string> users;

        public OnlineUserTracker()
        {
            users = new ConcurrentDictionary<string, string>();
        }
            
        public void PutUser(string user, string connectionId)
        {
            users.AddOrUpdate(user, connectionId, (u, c) => connectionId);
        }

        public void RemoveUser(string user)
        {
            users.TryRemove(user, out _);
        }

        public IEnumerable<string> GetUsers()
        {
            return users.Keys.ToList();
        }

        public bool IsOnline(string user)
        {
            return users.ContainsKey(user);
        }

        public string GetConnectionId(string user)
        {
            if (users.ContainsKey(user))
            {
                return users[user];
            }

            throw new KeyNotFoundException("The given user is not in the list of users.");
        }
    }
}