using System;

namespace Czeum.Core.DTOs.UserManagement
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}