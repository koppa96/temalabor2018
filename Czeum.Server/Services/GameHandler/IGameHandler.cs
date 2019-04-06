using System.Collections.Generic;
using Czeum.Abstractions.DTO;
using Czeum.DTO;

namespace Czeum.Server.Services.GameHandler
{
    public interface IGameHandler
    {
        Dictionary<string, MatchStatus> CreateMatch(LobbyData lobbyData);
        Dictionary<string, MatchStatus> HandleMove(MoveData moveData, int playerId);
        Dictionary<string, MatchStatus> CreateRandomMatch(string player1, string player2);
    }
}