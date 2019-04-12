using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.DTO.UserManagement;

namespace Czeum.Client.Interfaces
{
    public interface IUserManagerService
    {
        string AccessToken { get; }
        string Username { get; }

        Task<bool> LoginAsync(LoginModel data);
        Task LogOutAsync();
        Task ChangePasswordAsync(ChangePasswordModel data);
        Task RegisterAsync(RegisterModel data);
    }
}
