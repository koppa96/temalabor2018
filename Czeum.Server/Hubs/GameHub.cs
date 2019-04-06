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
using Czeum.Server.Services.GameHandler;
using Czeum.Server.Services.Lobby;
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
        private readonly IUnitOfWork _unitOfWork;
        
        public GameHub(IOnlineUserTracker onlineUserTracker, ILobbyService lobbyService, ILogger<GameHub> logger,
            ISoloQueueService soloQueueService, IGameHandler gameHandler, IUnitOfWork unitOfWork)
        {
            _onlineUserTracker = onlineUserTracker;
            _lobbyService = lobbyService;
            _logger = logger;
            _soloQueueService = soloQueueService;
            _gameHandler = gameHandler;
            _unitOfWork = unitOfWork;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            _onlineUserTracker.PutUser(Context.UserIdentifier);

            var friends = _unitOfWork.FriendRepository.GetFriendsOf(Context.UserIdentifier);
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

            var friends = _unitOfWork.FriendRepository.GetFriendsOf(Context.UserIdentifier);
            foreach (var friend in friends)
            {
                await Clients.User(friend).FriendDisconnected(Context.UserIdentifier);
            }
            
            _onlineUserTracker.RemoveUser(Context.UserIdentifier);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task ReceiveMove(MoveData moveData)
        {
            var match = _unitOfWork.MatchRepository.GetMatchById(moveData.MatchId);

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
            var match = _unitOfWork.MatchRepository.GetMatchById(matchId);
            if (!match.HasPlayer(Context.UserIdentifier))
            {
                await Clients.Caller.ReceiveError(ErrorCodes.CannotSendMessage);
                return;
            }
            
            _unitOfWork.MessageRepository.AddMessageNow(matchId, message);
            await Clients.User(match.GetOtherPlayerName(Context.UserIdentifier)).ReceiveMatchMessage(matchId, message);
        }
    }
}
