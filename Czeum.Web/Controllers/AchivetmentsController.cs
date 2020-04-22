using Czeum.Core.DTOs.Achivement;
using Czeum.Core.Services;
using Czeum.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czeum.Web.Controllers
{
    [Route(ApiResources.Achivements.BasePath)]
    [ApiController]
    [Authorize]
    public class AchivetmentsController : ControllerBase
    {
        private readonly IAchivementService achivementService;

        public AchivetmentsController(IAchivementService achivementService)
        {
            this.achivementService = achivementService;
        }

        [HttpGet]
        public Task<IEnumerable<AchivementDto>> GetAchivementsAsync()
        {
            return achivementService.GetAchivementsAsync();
        }

        [HttpPatch("{id}/star")]
        public Task<AchivementDto> StarAchivementAsync(Guid id)
        {
            return achivementService.StarAchivementAsync(id);
        }

        [HttpPatch("{id}/unstar")]
        public Task<AchivementDto> UnstarAchivementAsync(Guid id)
        {
            return achivementService.UnstarAchivementAsync(id);
        }
    }
}
