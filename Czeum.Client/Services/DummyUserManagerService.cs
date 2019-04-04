using Czeum.Client.Interfaces;
using Czeum.DTO.UserManagement;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Client.Services
{
    class DummyUserManagerService : IUserManagerService
    {
        public string AccessToken => throw new NotImplementedException();
        private ILoggerFacade _logger;
        private String username;

        public DummyUserManagerService(ILoggerFacade logger)
        {
            logger.Log("Dummy UMS initialized", Category.Debug, Priority.None);
            _logger = logger;
        }

        public async Task ChangePasswordAsync(ChangePasswordModel data)
        {
            _logger.Log($"Password change requested by user '{username}'", Category.Info, Priority.None);
        }

        public async Task<bool> LoginAsync(LoginModel data)
        {
            _logger.Log($"Login attempt by user '{data.Username}'", Category.Info, Priority.None);
            username = data.Username;
            return true;
        }

        public async Task RegisterAsync(RegisterModel data)
        {
            _logger.Log($"Registration requested by user '{data.Username}'", Category.Info, Priority.None);
        }
    }
}
