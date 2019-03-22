using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;
using Czeum.DAL.Entities;
using Czeum.DAL.Interfaces;
using Czeum.Server.Services;
using Czeum.Server.Services.Lobby;
using Czeum.Server.Services.OnlineUsers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Czeum.Server.Hubs
{
    public partial class GameHub : Hub<ICzeumClient>
    {
        private readonly IEnumerable<IGameService> _gameServices;
        private readonly IMatchRepository _matchRepository;
        private readonly IOnlineUserTracker _onlineUserTracker;
        private readonly ILobbyService _lobbyService;
        private readonly ILogger _logger;
        private readonly ISoloQueueService _soloQueueService;

        public GameHub(IEnumerable<IGameService> gameServices, IMatchRepository matchRepository, IOnlineUserTracker onlineUserTracker,
            ILobbyService lobbyService, ILogger<GameHub> logger, ISoloQueueService soloQueueService)
        {
            _gameServices = gameServices;
            _matchRepository = matchRepository;
            _onlineUserTracker = onlineUserTracker;
            _lobbyService = lobbyService;
            _logger = logger;
            _soloQueueService = soloQueueService;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            _onlineUserTracker.PutUser(Context.UserIdentifier);
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
            
            _onlineUserTracker.RemoveUser(Context.UserIdentifier);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task ReceiveMove(MoveData moveData)
        {
            var match = _matchRepository.GetMatchById(moveData.MatchId);
            
            if (!match.HasPlayer(Context.UserIdentifier))
            {
                await Clients.Caller.NotYourMatch();
                return;
            }

            if (!match.IsPlayersTurn(Context.UserIdentifier))
            {
                await Clients.Caller.NotYourTurn();
                return;
            }
            
            var playerId = match.GetPlayerId(Context.UserIdentifier);
            var service = moveData.FindGameService(_gameServices);
            var result = service.ExecuteMove(moveData, playerId);
            _matchRepository.UpdateMatchByStatus(match.MatchId, result.Status);
            
            await Clients.Caller.ReceiveResult(_matchRepository.CreateMatchStatusFor(moveData.MatchId, Context.UserIdentifier, result));
            if (result.Status != Status.Fail)
            {
                var otherPlayer = match.GetOtherPlayerName(Context.UserIdentifier);
                if (Clients.User(otherPlayer) != null)
                {
                    await Clients.User(otherPlayer)
                        .ReceiveResult(_matchRepository.CreateMatchStatusFor(moveData.MatchId, otherPlayer, result));
                }
            }
        }
    }
}
