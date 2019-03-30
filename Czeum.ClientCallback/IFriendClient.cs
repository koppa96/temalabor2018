using System.Threading.Tasks;
using Czeum.DTO.UserManagement;

namespace Czeum.ClientCallback
{
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