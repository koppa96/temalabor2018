using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Api.SignalR;
using Czeum.Application.Services;
using Microsoft.AspNetCore.SignalR;

namespace Czeum.Api.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        
        public Task NotifyAsync(string username, string @event, params object[] data)
        {
            return hubContext.Clients.User(username).SendAsync(@event, data);
        }

        public Task NotifyAllAsync(IEnumerable<string> usernames, string @event, params object[] data)
        {
            return Task.WhenAll(usernames.Select(u => NotifyAsync(u, @event, data)));
        }
    }
}