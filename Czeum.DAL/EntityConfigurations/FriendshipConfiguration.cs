using Czeum.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Czeum.DAL.EntityConfigurations
{
    public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
    {
        public void Configure(EntityTypeBuilder<Friendship> builder)
        {
            builder.HasOne(f => f.User1)
                .WithMany(u => u.User1Friendships)
                .HasForeignKey(f => f.User1Id);

            builder.HasOne(f => f.User2)
                .WithMany(u => u.User2Friendships)
                .HasForeignKey(f => f.User2Id);
        }
    }
}