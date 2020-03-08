using Czeum.Core.DTOs.Achivement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Core.Services
{
    public interface IAchivementService
    {
        Task<IEnumerable<AchivementDto>> GetAchivementsAsync();
        Task<AchivementDto> StarAchivementAsync(Guid userAchivementId);
        Task<AchivementDto> UnstarAchivementAsync(Guid userAchivementId);
    }
}
