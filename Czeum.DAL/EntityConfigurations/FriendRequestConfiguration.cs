using Czeum.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Czeum.DAL.EntityConfigurations
{
    public class FriendRequestConfiguration : IEntityTypeConfiguration<FriendRequest>
    {
        public void Configure(EntityTypeBuilder<FriendRequest> builder)
        {
            builder.HasOne(f => f.Sender)
                .WithMany(u => u.SentRequests)
                .HasForeignKey(f => f.SenderId);

            builder.HasOne(f => f.Receiver)
                .WithMany(u => u.ReceivedRequests)
                .HasForeignKey(f => f.ReceiverId);
        }
    }
}