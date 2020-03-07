using Czeum.Core.Domain;
using Czeum.Core.DTOs.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Domain.Entities
{
    public class Notification : EntityBase
    {
        public NotificationType Type { get; set; }
        public Guid? SenderUserId { get; set; }
        public User SenderUser { get; set; }

        public Guid ReceiverUserId { get; set; }
        public User ReceiverUser { get; set; }

        public Guid? Data { get; set; }
    }
}
