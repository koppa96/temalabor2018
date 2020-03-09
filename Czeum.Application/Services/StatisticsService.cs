using Czeum.Core.DTOs.Statistics;
using Czeum.Core.Enums;
using Czeum.Core.Services;
using Czeum.DAL;
using Czeum.Domain.Entities;
using Czeum.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.Core.DTOs.Extensions;

namespace Czeum.Application.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly CzeumContext context;
        private readonly IIdentityService identityService;
        private readonly IServiceContainer serviceContainer;

        public StatisticsService(CzeumContext context, IIdentityService identityService, IServiceContainer serviceContainer)
        {
            this.context = context;
            this.identityService = identityService;
            this.serviceContainer = serviceContainer;
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

        private GameType GetFavouriteGameType(User user)
        {
            var serializedBoardType = GetFavouriteBoardType(user);

            var moveHandlerType = serviceContainer.GetRegisteredMoveHandlerTypes()
                .First(x => x.GetGenericArguments().Last() == serializedBoardType);

            var moveDataType = moveHandlerType.GetGenericArguments().First();

            return Enum.GetValues(typeof(GameType)).Cast<GameType>()
                .First(x => x.GetGameTypeAttribute().MoveDataType == moveDataType);
        }
    }
}
