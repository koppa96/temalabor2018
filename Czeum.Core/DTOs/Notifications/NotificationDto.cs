using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Core.DTOs.Notifications
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public NotificationType Type { get; set; }
        public string SenderUserName { get; set; }
        public Guid? Data { get; set; }
    }
}
