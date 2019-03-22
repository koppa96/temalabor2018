using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Czeum.Server.Services.OnlineUsers
{
    public class OnlineUserTracker : IOnlineUserTracker
    {
        private readonly SynchronizedCollection<string> users;

        public OnlineUserTracker()
        {
            users = new SynchronizedCollection<string>();
        }
            
        public void PutUser(string user)
        {
            if (!users.Contains(user))
            {
                users.Add(user);
            }
        }

        public void RemoveUser(string user)
        {
            users.Remove(user);
        }

        public List<string> GetUsers()
        {
            return users.ToList();
        }

        public bool IsOnline(string user)
        {
            return users.Contains(user);
        }
    }
}