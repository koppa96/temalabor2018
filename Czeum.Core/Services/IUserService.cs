using System;
using System.Threading.Tasks;

namespace Czeum.Core.Services
{
    public interface IUserService
    {
        Task UpdateLastDisconnectDate(string username);
    }
}
