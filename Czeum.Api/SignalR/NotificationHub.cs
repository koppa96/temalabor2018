using System;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Core.ClientCallbacks;
using Czeum.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Czeum.Api.SignalR
{
    [Authorize]
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
            onlineUserTracker.PutUser(Context.UserIdentifier, Context.ConnectionId);

            await Task.WhenAll((await friendService.GetFriendsOfUserAsync(Context.UserIdentifier))
                .Select(f => Clients.User(f.Username).FriendConnected(f.FriendshipId)));
            
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var lobby = await lobbyService.GetLobbyOfUser(Context.UserIdentifier);

            if (lobby != null)
            {
                await lobbyService.DisconnectPlayerFromLobby(Context.UserIdentifier);
                if (await lobbyService.LobbyExists(lobby.Id))
                {
                    await Clients.All.LobbyChanged(await lobbyService.GetLobby(lobby.Id));
                }
                else
                {
                    await Clients.All.LobbyDeleted(lobby.Id);
                }
            }

            soloQueueService.LeaveSoloQueue(Context.UserIdentifier);

            await Task.WhenAll((await friendService.GetFriendsOfUserAsync(Context.UserIdentifier))
                .Select(f => Clients.User(f.Username).FriendDisconnected(f.FriendshipId)));

            onlineUserTracker.RemoveUser(Context.UserIdentifier);
            await base.OnDisconnectedAsync(exception);
        }
    }
}