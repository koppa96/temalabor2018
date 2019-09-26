using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.Application.Services.ServiceContainer;
using Czeum.DAL;
using Czeum.DAL.Entities;
using Czeum.DAL.Extensions;
using Czeum.DTO;
using Czeum.DTO.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Application.Services.GameHandler
{
    public class GameHandler : IGameHandler
    {
        private readonly IServiceContainer serviceContainer;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GameHandler(IServiceContainer serviceContainer, ApplicationDbContext context,
            IMapper mapper)
        {
            this.serviceContainer = serviceContainer;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Dictionary<string, MatchStatus>> CreateMatchAsync(LobbyData lobbyData)
        {
            var service = serviceContainer.FindByLobbyData(lobbyData);
            var board = (SerializedBoard) service.CreateNewBoard(lobbyData);

            return await CreateMatchWithBoardAsync(lobbyData.Host, lobbyData.Guest, board);
        }
        
        public async Task<Dictionary<string, MatchStatus>> CreateRandomMatchAsync(string player1, string player2)
        {
            var service = serviceContainer.GetRandomService();
            var board = (SerializedBoard) service.CreateDefaultBoard();

            return await CreateMatchWithBoardAsync(player1, player2, board);
        }

        private async Task<Dictionary<string, MatchStatus>> CreateMatchWithBoardAsync(string player1, string player2, SerializedBoard board)
        {
            var match = new Match
            {
                Board = board,
                Player1 = await context.Users.SingleAsync(u => u.UserName == player1),
                Player2 = await context.Users.SingleAsync(u => u.UserName == player2),
                State = MatchState.Player1Moves
            };

            context.Matches.Add(match);
            context.Boards.Add(board);
            await context.SaveChangesAsync();
            
            return new Dictionary<string, MatchStatus>
            {
                { player1, match.ToMatchStatus(player1) },
                { player2, match.ToMatchStatus(player2) }
            };
        }

        public async Task<Dictionary<string, MatchStatus>> HandleMoveAsync(MoveData moveData, string username)
        {
            var match = await context.Matches.FindAsync(moveData.MatchId);
            var service = serviceContainer.FindByMoveData(moveData);
            var board = await context.Boards.SingleAsync(b => b.Match.Id == moveData.MatchId);
            var result = service.ExecuteMove(moveData, match.GetPlayerId(username), board);
            
            board.BoardData = result.UpdatedBoardData;
            switch (result.Status)
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

            await context.SaveChangesAsync();

            var status1 = match.ToMatchStatus(match.Player1.UserName, mapper.Map<MoveResultWrapper>(result.MoveResult));
            var status2 = match.ToMatchStatus(match.Player2.UserName, mapper.Map<MoveResultWrapper>(result.MoveResult));

            return new Dictionary<string, MatchStatus>
            {
                { match.Player1.UserName, status1 },
                { match.Player2.UserName, status2 }
            };
        }

        public async Task<Match> GetMatchByIdAsync(Guid id)
        {
            return await context.Matches.Include(m => m.Player1)
                .Include(m => m.Player2)
                .SingleAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<MatchStatus>> GetMatchesOfPlayerAsync(string player)
        {
            var matches = await context.Matches
                .Include(m => m.Board)
                .Include(m => m.Player1)
                .Include(m => m.Player2)
                .Where(m => m.Player1.UserName == player || m.Player2.UserName == player)
                .ToListAsync();
            
            var statuses = new List<MatchStatus>();
            foreach (var match in matches)
            {
                var service = serviceContainer.FindBySerializedBoard(match.Board);
                var board = service.ConvertToMoveResult(match.Board);
                statuses.Add(match.ToMatchStatus(player, mapper.Map<MoveResultWrapper>(board)));
            }

            return statuses;
        }

        public async Task<MoveResultWrapper> GetBoardByMatchIdAsync(Guid matchId)
        {
            var board = await context.Boards.SingleAsync(b => b.Match.Id == matchId);
            var service = serviceContainer.FindBySerializedBoard(board);
            return mapper.Map<MoveResultWrapper>(service.ConvertToMoveResult(board));
        }
    }
}