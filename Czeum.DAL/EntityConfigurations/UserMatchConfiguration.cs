using Czeum.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Czeum.DAL.EntityConfigurations
{
    public class UserMatchConfiguration : IEntityTypeConfiguration<UserMatch>
    {
        public void Configure(EntityTypeBuilder<UserMatch> builder)
        {
            builder.HasOne(um => um.User)
                .WithMany(u => u.Matches)
                .HasForeignKey(um => um.UserId);

            builder.HasOne(um => um.Match)
                .WithMany(m => m.Users)
                .HasForeignKey(um => um.MatchId);
        }
    }
}