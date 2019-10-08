using Czeum.Core.DTOs;
using Czeum.Domain.Entities;

namespace Czeum.Application.Services
{
    public interface IMatchConverter
    {
        MatchStatus ConvertFor(Match match, string user);
    }
}