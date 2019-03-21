using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;
using Czeum.DAL.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Czeum.Server.Hubs
{
    public class GameHub : Hub<ICzeumClient>
    {
        private readonly IEnumerable<IGameService> _gameServices;
        private readonly IMatchRepository _matchRepository;

        public GameHub(IEnumerable<IGameService> gameServices, IMatchRepository matchRepository)
        {
            _gameServices = gameServices;
            _matchRepository = matchRepository;
        }

        public async Task ReceiveMove(MoveData moveData)
        {
            var match = _matchRepository.GetMatchById(moveData.MatchId);

            if (!match.HasPlayer(Context.UserIdentifier))
            {
                await Clients.Caller.NotYourMatch();
            }

            if (!match.IsPlayersTurn(Context.UserIdentifier))
            {
                await Clients.Caller.NotYourTurn();
            }

            var playerId = match.GetPlayerId(Context.UserIdentifier);
            var service = moveData.FindGameService(_gameServices);
            var result = service.ExecuteMove(moveData, playerId);
            _matchRepository.UpdateMatchByStatus(match.MatchId, result.Status);
            
            await Clients.Caller.ReceiveResult(_matchRepository.CreateMatchStatusFor(moveData.MatchId, Context.UserIdentifier, result));
            if (result.Status != Status.Fail)
            {
                var otherPlayer = _matchRepository.GetOtherPlayer(moveData.MatchId, Context.UserIdentifier);
                if (Clients.User(otherPlayer) != null)
                {
                    await Clients.User(otherPlayer)
                        .ReceiveResult(_matchRepository.CreateMatchStatusFor(moveData.MatchId, otherPlayer, result));
                }
            }
        }
    }
}
