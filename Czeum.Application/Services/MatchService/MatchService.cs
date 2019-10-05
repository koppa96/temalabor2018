using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.Application.Services.MatchConverter;
using Czeum.Application.Services.ServiceContainer;
using Czeum.DAL;
using Czeum.DAL.Extensions;
using Czeum.Domain.Entities;
using Czeum.Domain.Entities.Boards;
using Czeum.Domain.Services;
using Czeum.DTO;
using Czeum.DTO.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Application.Services.MatchService
{
    public class MatchService : IMatchService
    {
        private readonly IServiceContainer serviceContainer;
        private readonly CzeumContext context;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly IMatchConverter matchConverter;

        public MatchService(IServiceContainer serviceContainer, CzeumContext context,
            IMapper mapper, IIdentityService identityService, IMatchConverter matchConverter)
        {
            this.serviceContainer = serviceContainer;
            this.context = context;
            this.mapper = mapper;
            this.identityService = identityService;
            this.matchConverter = matchConverter;
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

            return match.Users.Select(um => matchConverter.ConvertFor(match, um.User.UserName));
        }

        public async Task<IEnumerable<MatchStatus>> HandleMoveAsync(MoveData moveData)
        {
            var userId = identityService.GetCurrentUserId();

            var match = await context.Matches.Include(m => m.Users)
                    .ThenInclude(um => um.User)
                .CustomSingleAsync(m => m.Id == moveData.MatchId, "No match with the given id was found.");

            var playerIndex = match.Users.SingleOrDefault(um => um.UserId == userId)?.PlayerIndex;
            if (playerIndex == null)
            {
                throw new UnauthorizedAccessException("You are not a player in this match.");
            }
            
            if (playerIndex != match.CurrentPlayerIndex)
            {
                throw new InvalidOperationException("Not your turn.");
            }

            var service = serviceContainer.FindMoveHandler(moveData);
            var result = await service.HandleAsync(moveData, playerIndex.Value);

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
            return match.Users.Select(um => matchConverter.ConvertFor(match, um.User.UserName));
        }

        public async Task<IEnumerable<MatchStatus>> GetMatchesAsync()
        {
            var currentUserName = identityService.GetCurrentUserName();

            return (await context.Matches.Include(m => m.Users)
                    .ThenInclude(um => um.User)
                .Include(m => m.Board)
                .Where(m => m.Users.Any(um => um.User.UserName == currentUserName))
                .ToListAsync())
                .Select(m => matchConverter.ConvertFor(m, currentUserName));
        }

        public async Task<IEnumerable<string>> GetOthersInMatchAsync(Guid matchId)
        {
            var currentUserId = identityService.GetCurrentUserId();
            return await context.UserMatches.Where(um => um.MatchId == matchId && um.UserId != currentUserId)
                .Select(um => um.User.UserName)
                .ToListAsync();
        }

        public async Task<MoveResultWrapper> GetBoardByMatchIdAsync(Guid matchId)
        {
            var board = await context.Boards.SingleAsync(b => b.Match.Id == matchId);
            var service = serviceContainer.FindBoardConverter(board);
            return mapper.Map<MoveResultWrapper>(service.Convert(board));
        }
    }
}