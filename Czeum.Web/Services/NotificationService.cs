using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Core.ClientCallbacks;
using Czeum.Core.Services;
using Czeum.Domain.Services;
using Czeum.Web.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace Czeum.Web.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub, ICzeumClient> hubContext;
        private readonly IOnlineUserTracker onlineUserTracker;

        public NotificationService(IHubContext<NotificationHub, ICzeumClient> hubContext,
            IOnlineUserTracker onlineUserTracker)
        {
            this.hubContext = hubContext;
            this.onlineUserTracker = onlineUserTracker;
        }

        public Task NotifyAsync(string client, Func<ICzeumClient, Task> action)
        {
            return action(hubContext.Clients.User(client));
        }

        public Task NotifyAsync(IEnumerable<string> clients, Func<ICzeumClient, Task> action)
        {
            return action(hubContext.Clients.Users(clients.ToList()));
        }

        public Task NotifyAllAsync(Func<ICzeumClient, Task> action)
        {
            return action(hubContext.Clients.All);
        }

        public Task NotifyEachAsync(IEnumerable<KeyValuePair<string, Func<ICzeumClient, Task>>> actions)
        {
            return Task.WhenAll(actions.Select(a => a.Value(hubContext.Clients.User(a.Key))));
        }

        public Task NotifyAllExceptAsync(string client, Func<ICzeumClient, Task> action)
        {
            var connectionId = onlineUserTracker.GetConnectionId(client);
            if (connectionId != null)
            {
                return action(hubContext.Clients.AllExcept(connectionId));
            }
            else
            {
                return Task.CompletedTask;
            }
        }
    }
}