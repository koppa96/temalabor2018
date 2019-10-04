using System.Collections.Generic;
using Czeum.Domain.Entities;
using Czeum.DTO;

namespace Czeum.Application.Services.MatchConverter
{
    public interface IMatchConverter
    {
        MatchStatus ConvertFor(Match match, string user);
    }
}