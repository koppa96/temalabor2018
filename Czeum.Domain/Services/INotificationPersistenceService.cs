using Czeum.Core.DTOs.Notifications;
using Czeum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Domain.Services
{
    public interface INotificationPersistenceService
    {
        Task PersistNotificationAsync(NotificationType notificationType, Guid receiverId, Guid? sender = null, Guid? data = null);
        Task PersistNotificationsAsync(NotificationType notificationType, IEnumerable<Guid> receiverIds, Guid? sender = null, Guid? data = null);
        Task RemoveNotificationsOf(Guid receiverId, Func<Notification, bool> predicate);
    }
}
