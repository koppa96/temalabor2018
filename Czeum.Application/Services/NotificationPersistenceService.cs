using Czeum.Core.DTOs.Notifications;
using Czeum.DAL;
using Czeum.Domain.Entities;
using Czeum.Domain.Services;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Application.Services
{
    public class NotificationPersistenceService : INotificationPersistenceService
    {
        private readonly CzeumContext context;
        private readonly INotificationService notificationService;

        public NotificationPersistenceService(CzeumContext context, INotificationService notificationService)
        {
            this.context = context;
            this.notificationService = notificationService;
        }

        public async Task PersistNotificationAsync(NotificationType notificationType, Guid receiverId, Guid? senderId = null, Guid? data = null)
        {
            var receiver = await context.Users.FindAsync(receiverId);            

            var notification = new Notification
            {
                Type = notificationType,
                ReceiverUserId = receiverId,
                SenderUserId = senderId,
                Data = data
            };

            context.Notifications.Add(notification);
            await context.SaveChangesAsync();
            await notificationService.NotifyAsync(receiver.UserName, async client => await client.NotificationReceived(new NotificationDto
            {
                Id = notification.Id,
                Data = notification.Data,
                SenderUserName = senderId != null ? (await context.Users.FindAsync(senderId)).UserName : null,
                Type = notificationType
            }));
        }

        public async Task PersistNotificationsAsync(NotificationType notificationType, IEnumerable<Guid> receiverIds, Guid? senderId = null, Guid? data = null)
        {
            var receivers = await context.Users.Where(x => receiverIds.ToList().Contains(x.Id)).ToListAsync();

            var notifications = receiverIds.Select(x => new Notification
            {
                Type = notificationType,
                ReceiverUserId = x,
                SenderUserId = senderId,
                Data = data
            });

            context.Notifications.AddRange(notifications);
            await context.SaveChangesAsync();

            var senderUserName = senderId != null ? (await context.Users.FindAsync(senderId)).UserName : null;
            await Task.WhenAll(
                notifications.Select(
                    x => notificationService.NotifyAsync(
                        receivers.Single(u => u.Id == x.ReceiverUserId).UserName,
                        client => client.NotificationReceived(new NotificationDto
                        {
                            Id = x.Id,
                            Data = x.Data,
                            SenderUserName = senderUserName,
                            Type = x.Type
                        })
                    )
                )
            );
        }

        public async Task RemoveNotificationsOf(Guid receiverId, Func<Notification, bool> predicate)
        {
            var userWithNotifications = await context.Users.Include(x => x.ReceivedNotifications)
                .SingleAsync(x => x.Id == receiverId);

            var notificationsToRemove = userWithNotifications.ReceivedNotifications.Where(predicate)
                .ToList();

            context.Notifications.RemoveRange(notificationsToRemove);
            await context.SaveChangesAsync();

            await Task.WhenAll(notificationsToRemove.Select(x => notificationService.NotifyAsync(userWithNotifications.UserName,
                client => client.NotificationCanceled(x.Id))));
        }

        public async Task RemoveNotificationsOf(IEnumerable<Guid> receiverIds, Func<Notification, bool> predicate)
        {
            var usersWithNotifications = await context.Users.Include(x => x.ReceivedNotifications)
                .Where(x => receiverIds.ToList().Contains(x.Id))
                .ToListAsync();

            foreach (var user in usersWithNotifications)
            {
                var notificationsToRemove = user.ReceivedNotifications.Where(predicate)
                    .ToList();

                context.Notifications.RemoveRange(notificationsToRemove);
                await Task.WhenAll(notificationsToRemove.Select(x => notificationService.NotifyAsync(user.UserName,
                    client => client.NotificationCanceled(x.Id))));
            }

            await context.SaveChangesAsync();
        }
    }
}
