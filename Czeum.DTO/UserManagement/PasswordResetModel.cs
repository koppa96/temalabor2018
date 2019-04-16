using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.DTO.UserManagement
{
    public class PasswordResetModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
