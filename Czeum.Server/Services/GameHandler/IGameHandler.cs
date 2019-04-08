using System.Collections.Generic;
using Czeum.Abstractions.DTO;
using Czeum.DAL.Entities;
using Czeum.DTO;

namespace Czeum.Server.Services.GameHandler
{
    public interface IGameHandler
    {
        Dictionary<string, MatchStatus> CreateMatch(LobbyData lobbyData);
        Dictionary<string, MatchStatus> CreateRandomMatch(string player1, string player2);
        Dictionary<string, MatchStatus> HandleMove(MoveData moveData, int playerId);
        Match GetMatchById(int id);
        List<MatchStatus> GetMatchesOf(string player);
        MoveResult GetBoardByMatchId(int matchId);
    }
}