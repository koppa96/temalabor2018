using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Statistics;
using Czeum.Core.GameServices.ServiceMappings;
using Czeum.Core.Services;
using Czeum.DAL;
using Czeum.Domain.Entities;
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
            var currentUser = await context.Users.AsNoTracking()
                .Include(x => x.Matches)
                    .ThenInclude(x => x.Match)
                        .ThenInclude(x => x.Users)
                            .ThenInclude(x => x.User)
                .SingleAsync(x => x.Id == currentUserId);

            var favouriteBoardType = GetFavouriteBoardType(currentUser);
            return new StatisticsDto
            {
                PlayedGames = currentUser.Matches.Count,
                WonGames = currentUser.WonMatches.Count,
                FavouriteGame = GetFavouriteGameType(currentUser),
                PlayedGamesOfFavourite = currentUser.Matches.Count(x => x.Match.Board.GetType() == favouriteBoardType),
                WonGamesOfFavourite = currentUser.WonMatches.Count(x => x.Board.GetType() == favouriteBoardType),
                MostPlayedWithName = GetFavouriteEnemy(currentUser).UserName
            };
        }

        private User GetFavouriteEnemy(User user)
        {
            return user.Matches.SelectMany(x => x.Match.Users.Where(u => u.UserId != x.Id).Select(x => x.User))
                .GroupBy(x => x.Id)
                .OrderByDescending(x => x.Count())
                .First()
                .First();
        }

        private Type GetFavouriteBoardType(User user)
        {
            return user.Matches.GroupBy(x => x.Match.Board.GetType())
                .OrderByDescending(x => x.Count())
                .First().Key;
        }

        private GameTypeDto GetFavouriteGameType(User user)
        {
            var serializedBoardType = GetFavouriteBoardType(user);

            var moveHandlerType = serviceContainer.GetRegisteredMoveHandlerTypes()
                .First(x => x.GetGenericArguments().Last() == serializedBoardType);

            var moveDataType = moveHandlerType.GetGenericArguments().First();

            var (identifier, displayName) = gameTypeMapping.GetDisplayDataBy(x => x.MoveDataType, moveDataType);
            return new GameTypeDto
            {
                Identifier = identifier,
                DisplayName = displayName
            };
        }
    }
}
