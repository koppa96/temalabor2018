using Czeum.Core.DTOs.Notifications;
using Czeum.DAL;
using Czeum.Domain.Entities;
using Czeum.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Application.Services
{
    public class NotificationPersistenceService : INotificationPersistenceService
    {
        private readonly CzeumContext context;

        public NotificationPersistenceService(CzeumContext context)
        {
            this.context = context;
        }

        public Task PersistNotificationAsync(NotificationType notificationType, Guid receiverId, Guid? sender = null, Guid? data = null)
        {
            var notification = new Notification
            {
                Type = notificationType,
                ReceiverUserId = receiverId,
                SenderUserId = sender,
                Data = data
            };

            context.Notifications.Add(notification);
            return context.SaveChangesAsync();
        }

        public Task PersistNotificationsAsync(NotificationType notificationType, IEnumerable<Guid> receiverIds, Guid? sender = null, Guid? data = null)
        {
            var notifications = receiverIds.Select(x => new Notification
            {
                Type = notificationType,
                ReceiverUserId = x,
                SenderUserId = sender,
                Data = data
            });

            context.Notifications.AddRange(notifications);
            return context.SaveChangesAsync();
        }
    }
}
