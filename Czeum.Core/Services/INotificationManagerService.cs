using Czeum.Core.DTOs.Notifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Core.Services
{
    public interface INotificationManagerService
    {
        Task<IEnumerable<NotificationDto>> GetNotificationsAsnyc();
        Task DeleteNotificationAsync(Guid notificationId);
    }
}
