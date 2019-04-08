using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;
using Czeum.DAL;
using Czeum.DAL.Entities;
using Czeum.DAL.Interfaces;
using Czeum.DTO;
using Czeum.Server.Services;
using Czeum.Server.Services.FriendService;
using Czeum.Server.Services.GameHandler;
using Czeum.Server.Services.Lobby;
using Czeum.Server.Services.MessageService;
using Czeum.Server.Services.OnlineUsers;
using Czeum.Server.Services.ServiceContainer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Czeum.Server.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public partial class GameHub : Hub<ICzeumClient>
    {
        private readonly IOnlineUserTracker _onlineUserTracker;
        private readonly ILobbyService _lobbyService;
        private readonly ILogger _logger;
        private readonly ISoloQueueService _soloQueueService;
        private readonly IGameHandler _gameHandler;
        private readonly IFriendService _friendService;
        private readonly IMessageService _messageService;
        
        public GameHub(IOnlineUserTracker onlineUserTracker, ILobbyService lobbyService, ILogger<GameHub> logger,
            ISoloQueueService soloQueueService, IGameHandler gameHandler, IFriendService friendService,
            IMessageService messageService)
        {
            _onlineUserTracker = onlineUserTracker;
            _lobbyService = lobbyService;
            _logger = logger;
            _soloQueueService = soloQueueService;
            _gameHandler = gameHandler;
            _friendService = friendService;
            _messageService = messageService;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            _onlineUserTracker.PutUser(Context.UserIdentifier);

            var friends = _friendService.GetFriendsOf(Context.UserIdentifier);
            foreach (var friend in friends)
            {
                await Clients.User(friend).FriendConnected(Context.UserIdentifier);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            
            var lobby = _lobbyService.FindUserLobby(Context.UserIdentifier);

            if (lobby != null)
            {
                _lobbyService.DisconnectPlayerFromLobby(Context.UserIdentifier, lobby.LobbyId);
                if (_lobbyService.LobbyExists(lobby.LobbyId))
                {
                    await Clients.All.LobbyChanged(_lobbyService.GetLobby(lobby.LobbyId));
                }
                else
                {
                    await Clients.All.LobbyDeleted(lobby.LobbyId);
                }
            }

            _soloQueueService.LeaveSoloQueue(Context.UserIdentifier);

            var friends = _friendService.GetFriendsOf(Context.UserIdentifier);
            foreach (var friend in friends)
            {
                await Clients.User(friend).FriendDisconnected(Context.UserIdentifier);
            }
            
            _onlineUserTracker.RemoveUser(Context.UserIdentifier);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task ReceiveMove(MoveData moveData)
        {
            var match = _gameHandler.GetMatchById(moveData.MatchId);

            if (match == null)
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NoSuchMatch);
                return;
            }
            
            if (!match.HasPlayer(Context.UserIdentifier))
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NotYourMatch);
                return;
            }

            if (!match.IsPlayersTurn(Context.UserIdentifier))
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NotYourTurn);
                return;
            }
            
            var playerId = match.GetPlayerId(Context.UserIdentifier);

            try
            {
                var result = _gameHandler.HandleMove(moveData, playerId);
                
                await Clients.Caller.ReceiveResult(result[Context.UserIdentifier]);
                if (result[Context.UserIdentifier].CurrentBoard.Status != Status.Fail)
                {
                    var otherPlayer = match.GetOtherPlayerName(Context.UserIdentifier);
                    await Clients.User(otherPlayer).ReceiveResult(result[otherPlayer]);
                }
            }
            catch (GameNotSupportedException)
            {
                await Clients.Caller.ReceiveError(ErrorCodes.GameNotSupported);
            }
        }

        public async Task SendMessageToMatch(int matchId, Message message)
        {
            if (!_messageService.SendToMatch(matchId, message, Context.UserIdentifier))
            {
                await Clients.Caller.ReceiveError(ErrorCodes.CannotSendMessage);
                return;
            }

            var match = _gameHandler.GetMatchById(matchId);
            await Clients.Caller.MatchMessageSent(matchId, message);
            await Clients.User(match.GetOtherPlayerName(Context.UserIdentifier)).ReceiveMatchMessage(matchId, message);
        }
    }
}
