using System;
using System.Threading.Tasks;
using Czeum.DTO.UserManagement;

namespace Czeum.ClientCallback
{
    /// <summary>
    /// An interface that the GameHub uses to notify its clients about events related to friends.
    /// </summary>
    public interface IFriendClient
    {
        Task ReceiveRequest(FriendRequestDto sender);
        Task RequestRejected(Guid requestId);
        Task FriendAdded(FriendDto friendDto);
        Task FriendRemoved(FriendDto friend);
        Task FriendConnected(Guid friendshipId);
        Task FriendDisconnected(Guid friendshipId);
    }
}