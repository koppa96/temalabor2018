using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Api.SignalR;
using Czeum.Application.Services;
using Czeum.ClientCallback;
using Microsoft.AspNetCore.SignalR;

namespace Czeum.Api.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub, ICzeumClient> hubContext;

        public NotificationService(IHubContext<NotificationHub, ICzeumClient> hubContext)
        {
            this.hubContext = hubContext;
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

        public Task NotifyEachAsync(Dictionary<string, Func<ICzeumClient, Task>> actions)
        {
            return Task.WhenAll(actions.Select(a => a.Value(hubContext.Clients.User(a.Key))));
        }

        public Task NotifyAllExceptAsync(string client, Func<ICzeumClient, Task> action)
        {
            return action(hubContext.Clients.AllExcept(client));
        }
    }
}