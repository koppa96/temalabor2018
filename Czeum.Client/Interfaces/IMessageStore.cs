using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Czeum.Core.DTOs;

namespace Czeum.Client.Interfaces
{
    public interface IMessageStore
    {
        ObservableCollection<Message> Messages { get; }

        Task SetMessages(IEnumerable<Message> messages);

        Task AddMessage(Message message);

    }
}