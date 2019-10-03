using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.Application.Extensions;
using Czeum.Application.Models;
using Czeum.Application.Services.ServiceContainer;
using Czeum.DAL;
using Czeum.DAL.Extensions;
using Czeum.Domain.Entities;
using Czeum.Domain.Entities.Boards;
using Czeum.Domain.Services;
using Czeum.DTO;
using Czeum.DTO.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Application.Services.GameHandler
{
    public class GameHandler : IGameHandler
    {
        private readonly IServiceContainer serviceContainer;
        private readonly CzeumContext context;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;

        public GameHandler(IServiceContainer serviceContainer, CzeumContext context,
            IMapper mapper, IIdentityService identityService)
        {
            this.serviceContainer = serviceContainer;
            this.context = context;
            this.mapper = mapper;
            this.identityService = identityService;
        }

        public Task<MatchStatusResult> CreateMatchAsync(LobbyData lobbyData)
        {
            var service = serviceContainer.FindBoardCreator(lobbyData);
            var board = (SerializedBoard)service.CreateBoard(lobbyData);

            return CreateMatchWithBoardAsync(lobbyData.Host, lobbyData.Guest, board);
        }
        
        public Task<MatchStatusResult> CreateRandomMatchAsync(string player1, string player2)
        {
            var service = serviceContainer.GetRandomBoardCreator();
            var board = (SerializedBoard)service.CreateDefaultBoard();

            return CreateMatchWithBoardAsync(player1, player2, board);
        }

        private async Task<MatchStatusResult> CreateMatchWithBoardAsync(string player1, string player2, SerializedBoard board)
        {
            var match = new Match
            {
                Board = board,
                Player1 = await context.Users.SingleAsync(u => u.UserName == player1),
                Player2 = await context.Users.SingleAsync(u => u.UserName == player2)
            };

            context.Matches.Add(match);
            context.Boards.Add(board);
            await context.SaveChangesAsync();

            var converter = serviceContainer.FindBoardConverter(board);
            return new MatchStatusResult(
                ConvertToMatchStatus(match, player1, converter.Convert(board)),
                ConvertToMatchStatus(match, player2, converter.Convert(board)));
        }

        public async Task<MatchStatusResult> HandleMoveAsync(MoveData moveData)
        {
            var currentUser = identityService.GetCurrentUserName();

            var match = await context.Matches.Include(m => m.Player1)
                    .Include(m => m.Player2)
                    .CustomSingleAsync(m => m.Id == moveData.MatchId, "No match with the given id was found.");

            var service = serviceContainer.FindMoveHandler(moveData);
            var result = await service.HandleAsync(moveData, match.GetPlayerId(currentUser));

            switch (result.Status)
            {
                case Status.Success:
                    match.NextTurn();
                    break;
                case Status.Win:
                    match.CurrentPlayerWon();
                    break;
                case Status.Draw:
                    match.Draw();
                    break;
            }

            await context.SaveChangesAsync();

            var status1 = ConvertToMatchStatus(match, match.Player1.UserName, result.MoveResult);
            var status2 = ConvertToMatchStatus(match, match.Player2.UserName, result.MoveResult);

            if (currentUser == match.Player1.UserName)
            {
                return new MatchStatusResult(status1, status2);
            } 
            else
            {
                return new MatchStatusResult(status2, status1);
            }
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
                var service = serviceContainer.FindBoardConverter(match.Board);
                var board = service.Convert(match.Board);
                statuses.Add(ConvertToMatchStatus(match, player, board));
            }

            return statuses;
        }

        public async Task<MoveResultWrapper> GetBoardByMatchIdAsync(Guid matchId)
        {
            var board = await context.Boards.SingleAsync(b => b.Match.Id == matchId);
            var service = serviceContainer.FindBoardConverter(board);
            return mapper.Map<MoveResultWrapper>(service.Convert(board));
        }

        private MatchStatus ConvertToMatchStatus(Match match,
            string player,
            IMoveResult currentBoard)
        {
            return new MatchStatus
            {
                Id = match.Id,
                State = match.GetGameStateForPlayer(player),
                OtherPlayer = match.GetOtherPlayerName(player),
                CurrentBoard = mapper.Map<MoveResultWrapper>(currentBoard),
                PlayerId = match.GetPlayerId(player)
            };
        }
    }
}