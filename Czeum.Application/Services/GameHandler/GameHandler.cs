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
using Czeum.Domain.Enums;
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

        public Task<IEnumerable<MatchStatus>> CreateMatchAsync(LobbyData lobbyData)
        {
            var service = serviceContainer.FindBoardCreator(lobbyData);
            var board = (SerializedBoard)service.CreateBoard(lobbyData);

            return CreateMatchWithBoardAsync(lobbyData.Guests.Append(lobbyData.Host), board);
        }
        
        public Task<IEnumerable<MatchStatus>> CreateRandomMatchAsync(IEnumerable<string> players)
        {
            var service = serviceContainer.GetRandomBoardCreator();
            var board = (SerializedBoard)service.CreateDefaultBoard();

            return CreateMatchWithBoardAsync(players, board);
        }

        private async Task<IEnumerable<MatchStatus>> CreateMatchWithBoardAsync(IEnumerable<string> players, SerializedBoard board)
        {
            var users = await context.Users.Where(u => players.Any(p => p == u.UserName))
                .ToListAsync();
            
            var match = new Match
            {
                Board = board,
                CurrentPlayerIndex = 0
            };

            match.Users = Enumerable.Range(0, users.Count)
                .Select(x => new UserMatch { User = users[x], Match = match, PlayerIndex = x })
                .ToList();

            context.Matches.Add(match);
            context.Boards.Add(board);
            await context.SaveChangesAsync();

            var converter = serviceContainer.FindBoardConverter(board);

            var playerList = match.Users.Select(um => new Player
                {Username = um.User.UserName, PlayerIndex = um.PlayerIndex});
            
            return match.Users.Select(um => new MatchStatus
            {
                Id = um.Match.Id,
                CurrentPlayerId = um.Match.CurrentPlayerIndex,
                Players = playerList,
                State = um.Match.State == MatchState.InProgress ? GameState.InProgress : GameState.Finished,
                Winner = null,
                CurrentBoard = converter.Convert()
            })
        }

        public async Task<IEnumerable<MatchStatus>> HandleMoveAsync(MoveData moveData)
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

        public async Task<IEnumerable<MatchStatus>> GetMatchesAsync()
        {
            var currentUserId = identityService.GetCurrentUserId();
            
            var matches = await context.Matches.Include(m => m.Users)
                .ThenInclude(um => um.User)
                .Include(m => m.Board)
                .Where(m => m.Users.Any(um => um.UserId == currentUserId))
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