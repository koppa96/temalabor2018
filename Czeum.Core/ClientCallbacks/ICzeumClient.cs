using Czeum.Core.DTOs.Notifications;
using System.Threading.Tasks;

namespace Czeum.Core.ClientCallbacks
{
    public interface ICzeumClient : IGameClient, ILobbyClient, IFriendClient, IMessageClient
    {
        Task NotificationReceived(NotificationDto dto);
    }
}
