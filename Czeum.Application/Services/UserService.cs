using System;
using System.Threading.Tasks;
using Czeum.Core.Services;
using Czeum.DAL;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Application.Services
{
    public class UserService : IUserService
    {
        private readonly CzeumContext context;

        public UserService(CzeumContext context)
        {
            this.context = context;
        }

        public async Task UpdateLastDisconnectDate(string username)
        {
            var user = await context.Users.SingleAsync(x => x.UserName == username);
            user.LastDisconnected = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }
    }
}
