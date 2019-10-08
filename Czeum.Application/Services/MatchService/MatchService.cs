using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.Application.Services.Lobby;
using Czeum.Application.Services.MatchConverter;
using Czeum.Application.Services.ServiceContainer;
using Czeum.ClientCallback;
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
        private readonly INotificationService notificationService;
        private readonly ILobbyStorage lobbyStorage;

        public MatchService(IServiceContainer serviceContainer, CzeumContext context,
            IMapper mapper, IIdentityService identityService, IMatchConverter matchConverter,
            INotificationService notificationService, ILobbyStorage lobbyStorage)
        {
            this.serviceContainer = serviceContainer;
            this.context = context;
            this.mapper = mapper;
            this.identityService = identityService;
            this.matchConverter = matchConverter;
            this.notificationService = notificationService;
            this.lobbyStorage = lobbyStorage;
        }

        public async Task<MatchStatus> CreateMatchAsync(Guid lobbyId)
        {
            var lobby = lobbyStorage.GetLobby(lobbyId);
            if (!lobby.Validate())
            {
                throw new InvalidOperationException("The lobby is not in a valid state to start a match.");
            }

            var currentUser = identityService.GetCurrentUserName();
            var service = serviceContainer.FindBoardCreator(lobby);
            var board = (SerializedBoard)service.CreateBoard(lobby);

            var statuses = await CreateMatchWithBoardAsync(lobby.Guests.Append(lobby.Host), board);
            lobbyStorage.RemoveLobby(lobbyId);
            
            await notificationService.NotifyEachAsync(statuses
                .Where(s => s.Key != currentUser)
                .Select(x => new KeyValuePair<string, Func<ICzeumClient, Task>>(x.Key, client => client.MatchCreated(x.Value))));

            return statuses.Single(s => s.Key == currentUser).Value;
        }
        
        public async Task CreateRandomMatchAsync(IEnumerable<string> players)
        {
            var service = serviceContainer.GetRandomBoardCreator();
            var board = (SerializedBoard)service.CreateDefaultBoard();

            var statues = await CreateMatchWithBoardAsync(players, board);
            await notificationService.NotifyEachAsync(statues
                .Select(x => new KeyValuePair<string,Func<ICzeumClient, Task>>(x.Key, client => client.MatchCreated(x.Value))));
        }

        private async Task<Dictionary<string, MatchStatus>> CreateMatchWithBoardAsync(IEnumerable<string> players, SerializedBoard board)
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

            return match.Users.Select(um => new { Player = um.User.UserName, Status = matchConverter.ConvertFor(match, um.User.UserName) })
                .ToDictionary(x => x.Player, x => x.Status);
        }

        public async Task<MatchStatus> HandleMoveAsync(MoveData moveData)
        {
            var currentUserId = identityService.GetCurrentUserId();
            var match = await context.Matches.Include(m => m.Users)
                    .ThenInclude(um => um.User)
                .CustomSingleAsync(m => m.Id == moveData.MatchId, "No match with the given id was found.");

            var playerIndex = match.Users.SingleOrDefault(um => um.UserId == currentUserId)?.PlayerIndex;
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
            await notificationService.NotifyEachAsync(match.Users
                .Where(um => um.UserId != currentUserId)
                .Select(um => new KeyValuePair<string, Func<ICzeumClient, Task>>(um.User.UserName, 
                    client => client.ReceiveResult(matchConverter.ConvertFor(match, um.User.UserName)))));

            return matchConverter.ConvertFor(match, 
                match.Users.Single(um => um.UserId == currentUserId).User.UserName);
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