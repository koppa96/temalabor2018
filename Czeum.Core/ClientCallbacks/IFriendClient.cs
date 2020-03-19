using System;
using System.Threading.Tasks;
using Czeum.Core.DTOs.UserManagement;

namespace Czeum.Core.ClientCallbacks
{
    /// <summary>
    /// An interface that the GameHub uses to notify its clients about events related to friends.
    /// </summary>
    public interface IFriendClient
    {
        Task ReceiveRequest(FriendRequestDto sender);
        Task RequestRejected(Guid requestId);
        Task RequestRevoked(Guid requestId);
        Task FriendAdded(FriendDto friendDto);
        Task FriendRemoved(Guid friendshipId);
        Task FriendConnectionStateChanged(FriendDto friendDto);
    }
}