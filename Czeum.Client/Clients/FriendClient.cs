using Czeum.Client.Interfaces;
using Czeum.Core.ClientCallbacks;
using Czeum.Core.DTOs.UserManagement;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Client.Clients
{
    public class FriendClient : IFriendClient
    {
        private IFriendStore friendStore;
        private IHubService hubService;

        public FriendClient(
            IFriendStore friendStore,
            IHubService hubService
        )
        {
            this.friendStore = friendStore;
            this.hubService = hubService;

            hubService.Connection.On<FriendDto>(nameof(FriendAdded), FriendAdded);
            hubService.Connection.On<Guid>(nameof(FriendConnected), FriendConnected);
            hubService.Connection.On<Guid>(nameof(FriendDisconnected), FriendDisconnected);
            hubService.Connection.On<Guid>(nameof(FriendRemoved), FriendRemoved);
            hubService.Connection.On<FriendRequestDto>(nameof(ReceiveRequest), ReceiveRequest);
            hubService.Connection.On<Guid>(nameof(RequestRejected), RequestRejected);
            hubService.Connection.On<Guid>(nameof(RequestRevoked), RequestRevoked);
        }


        public async Task FriendAdded(FriendDto friendDto)
        {
            await friendStore.AddFriend(friendDto);
            var request = friendStore.SentRequests.FirstOrDefault(r => r.ReceiverName == friendDto.Username);
            if (request != null) 
            {
                await friendStore.RemoveSentRequest(request.Id);
            }
        }

        public async Task FriendConnected(Guid friendshipId)
        {
            await friendStore.SetOnline(friendshipId, true);
        }

        public async Task FriendDisconnected(Guid friendshipId)
        {

            await friendStore.SetOnline(friendshipId, false);
        }

        public async Task FriendRemoved(Guid friendshipId)
        {
            await friendStore.RemoveFriend(friendshipId);
        }

        public async Task ReceiveRequest(FriendRequestDto sender)
        {
            await friendStore.AddReceivedRequest(sender);
        }

        public async Task RequestRejected(Guid requestId)
        {
            await friendStore.RemoveSentRequest(requestId);
        }

        public async Task RequestRevoked(Guid requestId)
        {
            await friendStore.RemoveReceivedRequest(requestId);
        }
    }
}
