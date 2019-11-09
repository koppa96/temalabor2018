using Czeum.Core.DTOs.UserManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Client.Interfaces
{
    public interface IFriendStore
    {
        ObservableCollection<FriendDto> Friends { get; }
        ObservableCollection<FriendRequestDto> SentRequests { get; }
        ObservableCollection<FriendRequestDto> ReceivedRequests{ get; }

        Task ClearFriends();
        Task AddFriends(IEnumerable<FriendDto> friends);
        Task AddSentRequests(IEnumerable<FriendRequestDto> sentRequests);
        Task ClearSentRequests();
        Task AddReceivedRequests(IEnumerable<FriendRequestDto> receivedRequests);
        Task ClearReceivedRequests();
        Task RemoveReceivedRequest(Guid value);
        Task RemoveFriend(Guid value);
        Task RemoveSentRequest(Guid value);
        Task AddFriend(FriendDto friendDto);
        Task AddReceivedRequest(FriendRequestDto sender);
    }
}
