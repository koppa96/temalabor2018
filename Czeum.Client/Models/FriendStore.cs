using Czeum.Client.Interfaces;
using Czeum.Core.DTOs.UserManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Czeum.Client.Models
{
    class FriendStore : IFriendStore
    {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        public ObservableCollection<FriendDto> Friends { get; private set; } = new ObservableCollection<FriendDto>();

        public ObservableCollection<FriendRequestDto> SentRequests { get; private set; } = new ObservableCollection<FriendRequestDto>();

        public ObservableCollection<FriendRequestDto> ReceivedRequests { get; private set; } = new ObservableCollection<FriendRequestDto>();

        public async Task AddFriends(IEnumerable<FriendDto> friends)
        {
            await ClearFriends();
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                foreach (var friend in friends)
                {
                    Friends.Add(friend);
                }
            });
        }

        public async Task ClearFriends()
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Friends.Clear();
            });
        }

        public async Task AddSentRequests(IEnumerable<FriendRequestDto> sentRequests)
        {

            await ClearSentRequests(); 
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                foreach (var request in sentRequests)
                {
                    SentRequests.Add(request);
                }
            });
        }

        public async Task ClearSentRequests()
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                SentRequests.Clear();
            });
        }

        public async Task AddReceivedRequests(IEnumerable<FriendRequestDto> receivedRequests)
        {

            await ClearReceivedRequests(); 
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                foreach (var request in receivedRequests)
                {
                    ReceivedRequests.Add(request);
                }
            });
        }

        public async Task ClearReceivedRequests()
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ReceivedRequests.Clear();
            });
        }

        public async Task RemoveReceivedRequest(Guid requestId)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var toRemove = ReceivedRequests.FirstOrDefault(r => r.Id == requestId);
                ReceivedRequests.Remove(toRemove);
            });
        }

        public async Task RemoveFriend(Guid friendshipId)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var toRemove = Friends.FirstOrDefault(f => f.FriendshipId == friendshipId);
                Friends.Remove(toRemove);
            });
        }

        public async Task RemoveSentRequest(Guid requestId)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var toRemove = SentRequests.FirstOrDefault(r => r.Id == requestId);
                SentRequests.Remove(toRemove);
            });
        }

        public async Task AddFriend(FriendDto friendDto)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Friends.Add(friendDto);
            });
        }

        public async Task AddReceivedRequest(FriendRequestDto sender)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ReceivedRequests.Add(sender);
            });
        }
    }
}
#pragma warning restore CS4014
