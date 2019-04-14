using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Server.Services.ServiceContainer;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;
using Czeum.DAL;
using Czeum.DAL.Entities;
using Czeum.DTO;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Server.Services.GameHandler
{
    public class GameHandler : IGameHandler
    {
        private readonly IServiceContainer _serviceContainer;
        private readonly IApplicationDbContext _context;

        public GameHandler(IServiceContainer serviceContainer, IApplicationDbContext context)
        {
            _serviceContainer = serviceContainer;
            _context = context;
        }

        public async Task<Dictionary<string, MatchStatus>> CreateMatchAsync(LobbyData lobbyData)
        {
            var service = _serviceContainer.FindByLobbyData(lobbyData);
            var board = (SerializedBoard) service.CreateNewBoard(lobbyData);

            return await CreateMatchWithBoardAsync(lobbyData.Host, lobbyData.Guest, board);
        }
        
        public async Task<Dictionary<string, MatchStatus>> CreateRandomMatchAsync(string player1, string player2)
        {
            var service = _serviceContainer.GetRandomService();
            var board = (SerializedBoard) service.CreateDefaultBoard();

            return await CreateMatchWithBoardAsync(player1, player2, board);
        }

        private async Task<Dictionary<string, MatchStatus>> CreateMatchWithBoardAsync(string player1, string player2, SerializedBoard board)
        {
            var match = new Match
            {
                Board = board,
                Player1 = await _context.Users.SingleAsync(u => u.UserName == player1),
                Player2 = await _context.Users.SingleAsync(u => u.UserName == player2),
                State = MatchState.Player1Moves
            };

            _context.Matches.Add(match);
            _context.Boards.Add(board);
            await _context.SaveChangesAsync();
            
            return new Dictionary<string, MatchStatus>
            {
                { player1, match.ToMatchStatus(player1) },
                { player2, match.ToMatchStatus(player2) }
            };
        }

        public async Task<Dictionary<string, MatchStatus>> HandleMoveAsync(MoveData moveData, int playerId)
        {
            var match = await _context.Matches.FindAsync(moveData.MatchId);
            var service = _serviceContainer.FindByMoveData(moveData);
            var board = await _context.Boards.SingleAsync(b => b.Match.MatchId == moveData.MatchId);
            var result = service.ExecuteMove(moveData, playerId, board);

            if (result.MoveResult.Status != Status.Fail || result.MoveResult.Status != Status.Requested)
            {
                board.BoardData = result.UpdatedBoardData;
                switch (result.MoveResult.Status)
                {
                    case Status.Success:
                        match.NextTurn();
                        break;
                    case Status.Win:
                        match.CurrentPlayerWon();
                        break;
                    case Status.Draw:
                        match.State = MatchState.Draw;
                        break;
                }

                await _context.SaveChangesAsync();
            }
            
            var status1 = match.ToMatchStatus(match.Player1.UserName);
            status1.CurrentBoard = result.MoveResult;
            var status2 = match.ToMatchStatus(match.Player2.UserName);
            status2.CurrentBoard = result.MoveResult;
            
            return new Dictionary<string, MatchStatus>
            {
                { match.Player1.UserName, status1 },
                { match.Player2.UserName, status2 }
            };
        }

        public async Task<Match> GetMatchByIdAsync(int id)
        {
            return await _context.Matches.Include("Player1")
                .Include("Player2")
                .SingleOrDefaultAsync(m => m.MatchId == id);
        }

        public async Task<List<MatchStatus>> GetMatchesOfPlayerAsync(string player)
        {
            return await _context.Matches.Where(m => m.Player1.UserName == player || m.Player2.UserName == player)
                .Select(m => m.ToMatchStatus(player))
                .ToListAsync();
        }

        public async Task<MoveResult> GetBoardByMatchIdAsync(int matchId)
        {
            var board = await _context.Boards.SingleAsync(b => b.Match.MatchId == matchId);
            var service = _serviceContainer.FindBySerializedBoard(board);
            return service.ConvertToMoveResult(board);
        }
    }
}