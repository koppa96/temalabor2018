using Czeum.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.DAL.EntityConfigurations
{
    public class UserAchivementConfiguration : IEntityTypeConfiguration<UserAchivement>
    {
        public void Configure(EntityTypeBuilder<UserAchivement> builder)
        {
            builder.HasOne(x => x.User)
                .WithMany(x => x.UserAchivements)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Achivement)
                .WithMany()
                .HasForeignKey(x => x.AchivementId);
        }
    }
}
