using System;

namespace Czeum.Core.DTOs.UserManagement
{
    /// <summary>
    /// Represents a friend of the user's friend list.
    /// </summary>
    public class FriendDto
    {
        public Guid FriendshipId { get; set; }
        public bool IsOnline { get; set; }
        public string Username { get; set; }
        public DateTime LastDisconnect { get; set; }
        public DateTime RegistrationTime { get; set; }
    }
}