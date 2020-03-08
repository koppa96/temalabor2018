using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Core.ClientCallbacks;

namespace Czeum.Domain.Services
{
    public interface INotificationService
    {
        Task NotifyAsync(string client, Func<ICzeumClient, Task> action);

        Task NotifyAsync(IEnumerable<string> clients, Func<ICzeumClient, Task> action);

        Task NotifyAllAsync(Func<ICzeumClient, Task> action);

        Task NotifyEachAsync(IEnumerable<KeyValuePair<string, Func<ICzeumClient, Task>>> actions);

        Task NotifyAllExceptAsync(string client, Func<ICzeumClient, Task> action);
    }
}