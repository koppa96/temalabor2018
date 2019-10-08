using System.Collections.Generic;
using System.Linq;
using Czeum.Domain.Entities;

namespace Czeum.Application.Extensions
{
    public static class MatchExtensions
    {
        public static IEnumerable<string> Others(this Match match, string player)
        {
            return match.Users.Select(um => um.User.UserName)
                .Where(n => n != player);
        }
    }
}