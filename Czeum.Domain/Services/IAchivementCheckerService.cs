using Czeum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Domain.Services
{
    public interface IAchivementCheckerService
    {
        Task<IEnumerable<UserAchivement>> CheckUnlockedAchivementsAsync(IEnumerable<User> users);
    }
}
