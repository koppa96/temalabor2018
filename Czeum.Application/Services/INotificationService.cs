using System.Collections.Generic;
using System.Threading.Tasks;

namespace Czeum.Application.Services
{
    public interface INotificationService
    {
        Task NotifyAsync(string username, string @event, params object[] data);
        Task NotifyAllAsync(IEnumerable<string> usernames, string @event, params object[] data);
    }
}