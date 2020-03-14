using Czeum.Core.DTOs.Statistics;
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
    [Route(ApiResources.Statistics.BasePath)]
    [ApiController]
    [Authorize]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        [HttpGet]
        public Task<StatisticsDto> GetStatisticsAsync()
        {
            return statisticsService.GetStatisticsAsync();
        }
    }
}
