using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Statistics;
using Czeum.Core.GameServices.ServiceMappings;
using Czeum.Core.Services;
using Czeum.DAL;
using Czeum.Domain.Entities;
using Czeum.Domain.Enums;
using Czeum.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Czeum.Application.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly CzeumContext context;
        private readonly IIdentityService identityService;
        private readonly IServiceContainer serviceContainer;
        private readonly IGameTypeMapping gameTypeMapping;

        public StatisticsService(
            CzeumContext context,
            IIdentityService identityService,
            IServiceContainer serviceContainer,
            IGameTypeMapping gameTypeMapping)
        {
            this.context = context;
            this.identityService = identityService;
            this.serviceContainer = serviceContainer;
            this.gameTypeMapping = gameTypeMapping;
        }

        public async Task<StatisticsDto> GetStatisticsAsync()
        {
            var currentUserId = identityService.GetCurrentUserId();
            var currentUser = await context.Users
                .Include(x => x.Matches)
                    .ThenInclude(x => x.Match)
                        .ThenInclude(x => x.Users)
                            .ThenInclude(x => x.User)
                .Include(x => x.Matches)
                    .ThenInclude(x => x.Match)
                        .ThenInclude(x => x.Board)
                .Include(x => x.WonMatches)
                .SingleAsync(x => x.Id == currentUserId);

            var favouriteBoardType = GetFavouriteBoardType(currentUser);
            return new StatisticsDto
            {
                PlayedGames = currentUser.Matches.Count(x => x.Match.State == MatchState.Finished),
                WonGames = currentUser.WonMatches?.Count ?? 0,
                FavouriteGame = GetFavouriteGameType(currentUser),
                PlayedGamesOfFavourite = favouriteBoardType != null ?
                    currentUser.Matches.Count(x => x.Match.Board.GetType() == favouriteBoardType && x.Match.State == MatchState.Finished) : 0,
                WonGamesOfFavourite = favouriteBoardType != null ?
                    currentUser.WonMatches.Count(x => x.Board.GetType() == favouriteBoardType) : 0,
                MostPlayedWithName = GetFavouriteEnemy(currentUser)?.UserName
            };
        }

        private User? GetFavouriteEnemy(User user)
        {
            return user.Matches.SelectMany(x => x.Match.Users.Select(m => m.User))
                .Where(x => x.Id != user.Id)
                .GroupBy(x => x.Id)
                .OrderByDescending(x => x.Count())
                .FirstOrDefault()?.First();
        }

        private Type? GetFavouriteBoardType(User user)
        {
            return user.Matches.GroupBy(x => x.Match.Board.GetType())
                .OrderByDescending(x => x.Count())
                .FirstOrDefault()?.Key;
        }

        private GameTypeDto? GetFavouriteGameType(User user)
        {
            var serializedBoardType = GetFavouriteBoardType(user);

            if (serializedBoardType != null)
            {
                var moveHandlerType = serviceContainer.GetRegisteredMoveHandlerTypes()
                    .First(x => x.BaseType!.GetGenericArguments().Last() == serializedBoardType);

                var moveDataType = moveHandlerType.BaseType!.GetGenericArguments().First();

                var (identifier, displayName) = gameTypeMapping.GetDisplayDataBy(x => x.MoveDataType, moveDataType);
                return new GameTypeDto
                {
                    Identifier = identifier,
                    DisplayName = displayName
                };
            }
            else
            {
                return null;
            }
        }
    }
}
