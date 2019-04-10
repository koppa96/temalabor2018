using Czeum.Client.Interfaces;
using Czeum.DTO.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Client.Services {
    class UserManagerService : IUserManagerService {
        public string AccessToken => throw new NotImplementedException();
        public string Username { get; }

        public Task LogOutAsync()
        {
            throw new NotImplementedException();
        }

        public Task ChangePasswordAsync(ChangePasswordModel data) {
            throw new NotImplementedException();
        }

        public Task<bool> LoginAsync(LoginModel data) {
            throw new NotImplementedException();
        }

        public Task RegisterAsync(RegisterModel data) {
            throw new NotImplementedException();
        }
    }
}
