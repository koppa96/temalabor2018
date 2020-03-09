using Czeum.Core.DTOs.Statistics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Core.Services
{
    public interface IStatisticsService
    {
        Task<StatisticsDto> GetStatisticsAsync();
    }
}
