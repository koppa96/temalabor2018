using System.Threading.Tasks;
using Czeum.DTO.UserManagement;

namespace Czeum.ClientCallback
{
    /// <summary>
    /// An interface that the GameHub uses to notify its clients about events related to friends.
    /// </summary>
    public interface IFriendClient
    {
        Task ReceiveRequest(string sender);
        Task RequestRejected(string receiver);
        Task SuccessfulRejection(string sender);
        Task SuccessfulRequest(string receiver);
        Task FriendAdded(Friend friend);
        Task FriendRemoved(string friend);
        Task FriendConnected(string friend);
        Task FriendDisconnected(string friend);
    }
}