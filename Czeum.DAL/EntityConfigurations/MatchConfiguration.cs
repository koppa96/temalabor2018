using Czeum.Domain.Entities;
using Czeum.Domain.Entities.Boards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Czeum.DAL.EntityConfigurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.HasOne(m => m.Board)
                .WithOne(b => b.Match)
                .HasForeignKey<SerializedBoard>(b => b.MatchId);

            builder.HasMany(m => m.Messages)
                .WithOne(sm => sm.Match)
                .HasForeignKey(sm => sm.MatchId);

            builder.HasOne(m => m.Winner)
                .WithMany(u => u.WonMatches)
                .HasForeignKey(m => m.WinnerId);
        }
    }
}