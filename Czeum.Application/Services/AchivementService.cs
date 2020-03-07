using AutoMapper;
using Czeum.Core.DTOs.Achivement;
using Czeum.Core.Services;
using Czeum.DAL;
using Czeum.DAL.Extensions;
using Czeum.Domain.Entities;
using Czeum.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Application.Services
{
    public class AchivementService : IAchivementService
    {
        private readonly CzeumContext context;
        private readonly IIdentityService identityService;
        private readonly IMapper mapper;

        public AchivementService(CzeumContext context, IIdentityService identityService, IMapper mapper)
        {
            this.context = context;
            this.identityService = identityService;
            this.mapper = mapper;
        }

        public async Task CheckUnlockedAchivementsAsync()
        {
            var currentUserId = identityService.GetCurrentUserId();
            var user = await context.Users.Include(x => x.UserAchivements)
                .Include(x => x.Matches)
                    .ThenInclude(x => x.Match)
                        .ThenInclude(x => x.Board)
                .SingleAsync(x => x.Id == currentUserId);

            var unfullfilledAchivements = await context.Achivements
                .Where(x => !user.UserAchivements.Select(x => x.AchivementId).ToList().Contains(x.Id))
                .ToListAsync();

            foreach (var achivement in unfullfilledAchivements)
            {
                if (achivement.CheckCriteria(user))
                {
                    var userAchivement = new UserAchivement
                    {
                        Achivement = achivement,
                        UnlockedAt = DateTime.UtcNow,
                        IsStarred = false,
                        User = user
                    };

                    context.UserAchivements.Add(userAchivement);
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AchivementDto>> GetAchivementsAsync()
        {
            var currentUserId = identityService.GetCurrentUserId();
            var userAchivements = await context.UserAchivements
                .AsNoTracking()
                .Include(x => x.Achivement)
                .Where(x => x.UserId == currentUserId)
                .ToListAsync();

            return mapper.Map<List<AchivementDto>>(userAchivements);
        }

        public async Task<AchivementDto> StarAchivementAsync(Guid userAchivementId)
        {
            var currentUserId = identityService.GetCurrentUserId();
            var userAchivement = await context.UserAchivements
                .Include(x => x.Achivement)
                .CustomSingleAsync(x => x.Id == userAchivementId, "No such achivement exists.");

            if (userAchivement.UserId != currentUserId)
            {
                throw new UnauthorizedAccessException("Can not star other user's achivements.");
            }

            userAchivement.IsStarred = true;
            await context.SaveChangesAsync();

            return mapper.Map<AchivementDto>(userAchivement);
        }

        public async Task<AchivementDto> UnstarAchivementAsync(Guid userAchivementId)
        {
            var currentUserId = identityService.GetCurrentUserId();
            var userAchivement = await context.UserAchivements
                .Include(x => x.Achivement)
                .CustomSingleAsync(x => x.Id == userAchivementId, "No such achivement exists.");

            if (userAchivement.UserId != currentUserId)
            {
                throw new UnauthorizedAccessException("Can not unstar other user's achivements.");
            }

            userAchivement.IsStarred = false;
            await context.SaveChangesAsync();

            return mapper.Map<AchivementDto>(userAchivement);
        }
    }
}
