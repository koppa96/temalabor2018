using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Core.Services;

namespace Czeum.Application.Services
{
    public class OnlineUserTracker : IOnlineUserTracker
    {
        private readonly ConcurrentDictionary<string, string> users;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<bool>> leavingUsers;

        public OnlineUserTracker()
        {
            users = new ConcurrentDictionary<string, string>();
            leavingUsers = new ConcurrentDictionary<string, TaskCompletionSource<bool>>();
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

        public string? GetConnectionId(string user)
        {
            if (users.ContainsKey(user))
            {
                return users[user];
            }

            return null;
        }

        public Task<bool> WaitTimeout(string username)
        {
            if (leavingUsers.ContainsKey(username))
            {
                // Ha már folyamatban van egy timeout ne csináljon semmit
                return Task.FromResult(true);
            }
            
            var completionSource = new TaskCompletionSource<bool>();
            leavingUsers.TryAdd(username, completionSource);

            Task.Run(async () =>
            {
                await Task.Delay(10000);
                if (!completionSource.Task.IsCompleted)
                {
                    completionSource.SetResult(false);
                    leavingUsers.TryRemove(username, out _);
                }
            });

            return completionSource.Task;
        }

        public bool OnReconnect(string username)
        {
            if (!leavingUsers.ContainsKey(username))
            {
                return false;
            }

            leavingUsers[username].SetResult(true);
            leavingUsers.TryRemove(username, out _);
            return true;
        }
    }
}