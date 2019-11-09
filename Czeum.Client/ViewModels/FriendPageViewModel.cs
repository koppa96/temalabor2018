using Czeum.Client.Interfaces;
using Czeum.Core.ClientCallbacks;
using Czeum.Core.DTOs.UserManagement;
using Czeum.Core.Services;
using Flurl.Http;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Czeum.Client.ViewModels
{
    public class FriendPageViewModel : ViewModelBase
    {
        public IFriendStore friendStore { get; private set; }
        private IFriendService friendService;
        private IUserManagerService userManagerService;
        private IDialogService dialogService;
        private IFriendClient friendClient;

        public string FriendSearchName { get; set; }
        public ICommand SearchFriendCommand { get; set; }
        public ICommand AddFriendCommand { get; set; }
        public ICommand CancelRequestCommand { get; set; }
        public ICommand AcceptRequestCommand { get; set; }
        public ICommand RejectRequestCommand { get; set; }
        public ICommand RemoveFriendCommand { get; set; }



        public ObservableCollection<UserDto> FoundUsers { get; private set; } = new ObservableCollection<UserDto>();

        public FriendPageViewModel(
            IFriendStore friendStore, 
            IFriendService friendService,
            IUserManagerService userManagerService,
            IDialogService dialogService,
            IFriendClient friendClient)
        {
            this.friendStore = friendStore;
            this.friendService = friendService;
            this.userManagerService = userManagerService;
            this.dialogService = dialogService;
            this.friendClient = friendClient;

            SearchFriendCommand = new DelegateCommand(SearchFriend);
            AddFriendCommand = new DelegateCommand<Guid?>(AddFriend);
            CancelRequestCommand = new DelegateCommand<Guid?>(CancelRequest);
            AcceptRequestCommand = new DelegateCommand<Guid?>(AcceptRequest);
            RejectRequestCommand = new DelegateCommand<Guid?>(RejectRequest);
            RemoveFriendCommand = new DelegateCommand<Guid?>(RemoveFriend);
        }

        private async void RemoveFriend(Guid? friendshipId)
        {
            if (!friendshipId.HasValue)
            {
                return;
            }
            try
            {
                await friendService.RemoveFriendAsync(friendshipId.Value);
                await friendStore.RemoveFriend(friendshipId.Value);
            }
            catch (FlurlHttpException e)
            {
                await dialogService.ShowError("Failed to cancel request.");
            }
        }

        private async void RejectRequest(Guid? requestId)
        {
            if (!requestId.HasValue)
            {
                return;
            }
            try
            {
                await friendService.RejectRequestAsync(requestId.Value);
                await friendStore.RemoveReceivedRequest(requestId.Value);
            }
            catch (FlurlHttpException e)
            {
                await dialogService.ShowError("Failed to cancel request.");
            }
        }

        private async void AcceptRequest(Guid? requestId)
        {
            if (!requestId.HasValue)
            {
                return;
            }
            try
            {
                var friend = await friendService.AcceptRequestAsync(requestId.Value);
                await friendStore.RemoveReceivedRequest(requestId.Value);
                friendStore.Friends.Add(friend);
            }
            catch (FlurlHttpException e)
            {
                await dialogService.ShowError("Failed to cancel request.");
            }
        }

        private async void CancelRequest(Guid? requestId)
        {
            if (!requestId.HasValue)
            {
                return;
            }
            try
            {
                await friendService.RevokeRequestAsync(requestId.Value);
                await friendStore.RemoveSentRequest(requestId.Value);
            }
            catch (FlurlHttpException e)
            {
                await dialogService.ShowError("Failed to cancel request.");
            }
        }

        private async void AddFriend(Guid? userId)
        {
            if (!userId.HasValue)
            {
                return;
            }
            try
            {
                var request = await friendService.AddRequestAsync(userId.Value);
                friendStore.SentRequests.Add(request);
            }
            catch(FlurlHttpException e)
            {
                await dialogService.ShowError("Failed to send request.");
            }
        }

        private async void SearchFriend()
        {
            if (string.IsNullOrEmpty(FriendSearchName))
            {
                return;
            }
            try
            {
                var searchResult = await userManagerService.SearchUsersAsync(FriendSearchName);
                FoundUsers.Clear();
                foreach(var dto in searchResult)
                {
                    FoundUsers.Add(dto);
                }
            }
            catch(FlurlHttpException e)
            {
                await dialogService.ShowError("Could not search");
            }
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            try
            {
                var sentRequests = await friendService.GetRequestsSentAsync();
                await friendStore.AddSentRequests(sentRequests);
                var receivedRequests = await friendService.GetRequestsReceivedAsync();
                await friendStore.AddReceivedRequests(receivedRequests);
                var friends = await friendService.GetFriendsOfUserAsync(null);
                await friendStore.AddFriends(friends);
            }
            catch(FlurlHttpException ex)
            {
                
            }
        }
    }
}
