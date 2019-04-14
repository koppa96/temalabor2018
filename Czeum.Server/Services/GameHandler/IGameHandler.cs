using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.DAL.Entities;
using Czeum.DTO;

namespace Czeum.Server.Services.GameHandler
{
    public interface IGameHandler
    {
        Task<Dictionary<string, MatchStatus>> CreateMatchAsync(LobbyData lobbyData);
        Task<Dictionary<string, MatchStatus>> CreateRandomMatchAsync(string player1, string player2);
        Task<Dictionary<string, MatchStatus>> HandleMoveAsync(MoveData moveData, int playerId);
        Task<Match> GetMatchByIdAsync(int id);
        Task<List<MatchStatus>> GetMatchesOfPlayerAsync(string player);
        Task<MoveResult> GetBoardByMatchIdAsync(int matchId);
    }
}