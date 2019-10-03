using Czeum.Domain.Entities.Boards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Czeum.DAL.EntityConfigurations
{
    public class ChessConfiguration : IEntityTypeConfiguration<SerializedChessBoard>
    {
        public void Configure(EntityTypeBuilder<SerializedChessBoard> builder)
        {
            builder.HasBaseType<SerializedBoard>();
        }
    }
}