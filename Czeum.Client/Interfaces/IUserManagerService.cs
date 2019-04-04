using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.DTO.UserManagement;

namespace Czeum.Client.Interfaces
{
    interface IUserManagerService
    {
        string AccessToken { get; }
        
        Task<bool> LoginAsync(LoginModel data);
        Task ChangePasswordAsync(ChangePasswordModel data);
        Task RegisterAsync(RegisterModel data);
    }
}
