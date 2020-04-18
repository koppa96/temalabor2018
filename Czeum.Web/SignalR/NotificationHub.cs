using System;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Core.ClientCallbacks;
using Czeum.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Czeum.Web.SignalR
{
    [Authorize]
    public class NotificationHub : Hub<ICzeumClient>
    {
        private readonly IOnlineUserTracker onlineUserTracker;
        private readonly IFriendService friendService;
        private readonly ISoloQueueService soloQueueService;
        private readonly ILobbyService lobbyService;
        private readonly IUserService userService;
        private readonly ILogger<NotificationHub> logger;

        public NotificationHub(IOnlineUserTracker onlineUserTracker,
            IFriendService friendService,
            ISoloQueueService soloQueueService,
            ILobbyService lobbyService,
            IUserService userService,
            ILogger<NotificationHub> logger)
        {
            this.onlineUserTracker = onlineUserTracker;
            this.friendService = friendService;
            this.soloQueueService = soloQueueService;
            this.lobbyService = lobbyService;
            this.userService = userService;
            this.logger = logger;
        }
        
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            var reconnected = onlineUserTracker.OnReconnect(Context.UserIdentifier);
            if (reconnected)
            {
                logger.LogInformation($"{Context.UserIdentifier} has reconnected within the timeout.");
                return;
            }

            logger.LogInformation($"{Context.UserIdentifier} has connected.");
            onlineUserTracker.PutUser(Context.UserIdentifier, Context.ConnectionId);

            await Task.WhenAll((await friendService.GetNotificationDataAsync(Context.UserIdentifier))
                .Select(x => Clients.User(x.Key).FriendConnectionStateChanged(x.Value)));
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            logger.LogInformation($"Lost connection to {Context.UserIdentifier}. Waiting for reconnect...");
            var waitForReconnect = await onlineUserTracker.WaitTimeout(Context.UserIdentifier);
            if (waitForReconnect)
            {
                return;
            }

            logger.LogInformation($"{Context.UserIdentifier} failed to reconnect within the timeout period.");
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

            await userService.UpdateLastDisconnectDate(Context.UserIdentifier);

            onlineUserTracker.RemoveUser(Context.UserIdentifier);

            await Task.WhenAll((await friendService.GetNotificationDataAsync(Context.UserIdentifier))
                .Select(x => Clients.User(x.Key).FriendConnectionStateChanged(x.Value)));

            await base.OnDisconnectedAsync(exception);
        }
    }
}