using Czeum.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.DAL.EntityConfigurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasOne(x => x.SenderUser)
                .WithMany()
                .HasForeignKey(x => x.SenderUserId);

            builder.HasOne(x => x.ReceiverUser)
                .WithMany(x => x.ReceivedNotifications)
                .HasForeignKey(x => x.ReceiverUserId);
        }
    }
}
