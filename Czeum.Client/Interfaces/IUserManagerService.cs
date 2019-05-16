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
        Task<bool> LogOutAsync();
        Task<bool> ChangePasswordAsync(ChangePasswordModel data);
        Task<bool> RegisterAsync(RegisterModel data);
        Task<bool> ConfirmAsync(string name, string confirmationToken);
    }
}
