using System;

namespace Czeum.DTO.UserManagement
{
    /// <summary>
    /// Represents a friend of the user's friend list.
    /// </summary>
    public class FriendDto
    {
        public Guid FriendshipId { get; set; }
        public bool IsOnline { get; set; }
        public string Username { get; set; }
    }
}