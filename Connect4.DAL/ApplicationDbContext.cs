using System;
using Connect4.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Connect4.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Match> Matches { get; set; }
        public DbSet<SerializedBoard> Boards { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

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
