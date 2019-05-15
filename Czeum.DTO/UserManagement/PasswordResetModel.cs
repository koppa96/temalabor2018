using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.DTO.UserManagement
{
    /// <summary>
    /// The data used by the AccountController to reset the user's password.
    /// </summary>
    public class PasswordResetModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
