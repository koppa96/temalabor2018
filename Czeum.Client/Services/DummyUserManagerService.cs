using Czeum.Client.Interfaces;
using Czeum.DTO.UserManagement;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Czeum.Client.Services
{
    class DummyUserManagerService : IUserManagerService
    {
        public string AccessToken => throw new NotImplementedException();
        public string Username { get; private set; }

        private ILoggerFacade _logger;


        public DummyUserManagerService(ILoggerFacade logger)
        {
            logger.Log("Dummy UMS initialized", Category.Debug, Priority.None);
            _logger = logger;
        }

        public async Task<bool> LogOutAsync()
        {
            Username = null;
            return true;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordModel data)
        {
            _logger.Log($"Password change requested by user '{Username}'", Category.Info, Priority.None);
            return true;
        }

        public async Task<bool> LoginAsync(LoginModel data)
        {
            _logger.Log($"Login attempt by user '{data.Username}'", Category.Info, Priority.None);
            await Task.Delay(2000);
            Username = data.Username;
            return true;
        }

        public async Task<bool> RegisterAsync(RegisterModel data)
        {
            _logger.Log($"Registration requested by user '{data.Username}'", Category.Info, Priority.None);
            await Task.Delay(2000);
            return true;
        }

        public Task<bool> ConfirmAsync(string name, string confirmationToken)
        {
            throw new NotImplementedException();
        }
    }
}
