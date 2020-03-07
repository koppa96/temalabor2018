using Czeum.Core.DTOs.Notifications;
using Czeum.Core.Services;
using Czeum.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czeum.Web.Controllers
{
    [Route(ApiResources.Notifications.BasePath)]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationManagerService notificationManagerService;

        public NotificationsController(INotificationManagerService notificationManagerService)
        {
            this.notificationManagerService = notificationManagerService;
        }

        [HttpGet]
        public Task<IEnumerable<NotificationDto>> GetNotificationsAsync()
        {
            return notificationManagerService.GetNotificationsAsnyc();
        }

        [HttpDelete("{id}")]
        public Task DeleteNotificationAsync(Guid id)
        {
            return notificationManagerService.DeleteNotificationAsync(id);
        }
    }
}
