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
    public class AchivementService : IAchivementService, IAchivementCheckerService
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

        public async Task<IEnumerable<UserAchivement>> CheckUnlockedAchivementsAsync(IEnumerable<User> users)
        {
            var achivements = await context.Achivements
                .ToListAsync();

            var achivementList = new List<UserAchivement>();
            foreach (var achivement in achivements)
            {
                foreach (var user in users)
                {
                    if (user.UserAchivements.All(x => x.AchivementId != achivement.Id) && achivement.CheckCriteria(user))
                    {
                        achivementList.Add(new UserAchivement
                        {
                            Achivement = achivement,
                            UnlockedAt = DateTime.UtcNow,
                            IsStarred = false,
                            User = user
                        });
                    }
                }

            }

            return achivementList;
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
