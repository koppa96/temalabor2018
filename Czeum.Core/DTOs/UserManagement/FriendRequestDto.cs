using System;

namespace Czeum.Core.DTOs.UserManagement
{
    public class FriendRequestDto
    {
        public Guid Id { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
    }
}