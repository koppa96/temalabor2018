using System;
using System.Threading.Tasks;
using Czeum.Application.Services.FriendService;
using Czeum.Application.Services.Lobby;
using Czeum.Application.Services.OnlineUsers;
using Czeum.Application.Services.SoloQueue;
using Microsoft.AspNetCore.SignalR;

namespace Czeum.Api.SignalR
{
    public class NotificationHub : Hub<ICzeumClient>
    {
        private readonly IOnlineUserTracker onlineUserTracker;
        private readonly IFriendService friendService;
        private readonly ISoloQueueService soloQueueService;
        private readonly ILobbyService lobbyService;

        public NotificationHub(IOnlineUserTracker onlineUserTracker,
            IFriendService friendService,
            ISoloQueueService soloQueueService,
            ILobbyService lobbyService)
        {
            this.onlineUserTracker = onlineUserTracker;
            this.friendService = friendService;
            this.soloQueueService = soloQueueService;
            this.lobbyService = lobbyService;
        }
        
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            onlineUserTracker.PutUser(Context.UserIdentifier);

            var friends = await friendService.GetFriendsOfUserAsync(Context.UserIdentifier);
            foreach (var friend in friends)
            {
                await Clients.User(friend).FriendConnected(Context.UserIdentifier);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var lobby = lobbyService.GetLobbyOfUser(Context.UserIdentifier);

            if (lobby != null)
            {
                lobbyService.DisconnectPlayerFromLobby(Context.UserIdentifier);
                if (lobbyService.LobbyExists(lobby.Id))
                {
                    await Clients.All.LobbyChanged(lobbyService.GetLobby(lobby.Id));
                }
                else
                {
                    await Clients.All.LobbyDeleted(lobby.Id);
                }
            }

            soloQueueService.LeaveSoloQueue(Context.UserIdentifier);

            var friends = await friendService.GetFriendsOfUserAsync(Context.UserIdentifier);
            foreach (var friend in friends)
            {
                await Clients.User(friend).FriendDisconnected(Context.UserIdentifier);
            }
            
            onlineUserTracker.RemoveUser(Context.UserIdentifier);
            await base.OnDisconnectedAsync(exception);
        }
    }
}