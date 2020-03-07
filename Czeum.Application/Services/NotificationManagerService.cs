using AutoMapper;
using Czeum.Core.DTOs.Notifications;
using Czeum.Core.Services;
using Czeum.DAL;
using Czeum.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Application.Services
{
    public class NotificationManagerService : INotificationManagerService
    {
        private readonly CzeumContext context;
        private readonly IIdentityService identityService;
        private readonly IMapper mapper;

        public NotificationManagerService(CzeumContext context, IIdentityService identityService, IMapper mapper)
        {
            this.context = context;
            this.identityService = identityService;
            this.mapper = mapper;
        }

        public async Task DeleteNotificationAsync(Guid notificationId)
        {
            var notification = await context.Notifications.FindAsync(notificationId);
            context.Notifications.Remove(notification);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<NotificationDto>> GetNotificationsAsnyc()
        {
            var currentUserId = identityService.GetCurrentUserId();
            var notifications = await context.Notifications
                .AsNoTracking()
                .Include(x => x.SenderUser)
                .Where(x => x.ReceiverUserId == currentUserId)
                .ToListAsync();

            return mapper.Map<List<NotificationDto>>(notifications);
        }
    }
}
