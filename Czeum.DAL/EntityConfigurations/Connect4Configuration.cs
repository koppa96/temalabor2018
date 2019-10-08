using Czeum.Core.Domain;
using Czeum.Domain.Entities.Boards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Czeum.DAL.EntityConfigurations
{
    public class Connect4Configuration : IEntityTypeConfiguration<SerializedConnect4Board>
    {
        public void Configure(EntityTypeBuilder<SerializedConnect4Board> builder)
        {
            builder.HasBaseType<SerializedBoard>();
        }
    }
}