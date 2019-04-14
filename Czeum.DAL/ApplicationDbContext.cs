using System;
using Czeum.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Czeum.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Match> Matches { get; set; }
        public DbSet<SerializedBoard> Boards { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<FriendRequest> Requests { get; set; }
        public DbSet<StoredMessage> Messages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SerializedConnect4Board>();
            builder.Entity<SerializedChessBoard>();

            base.OnModelCreating(builder);
        }
    }
}
