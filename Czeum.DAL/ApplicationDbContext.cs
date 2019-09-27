using System;
using Czeum.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Czeum.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<Match> Matches { get; set; }
        public DbSet<SerializedBoard> Boards { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<FriendRequest> Requests { get; set; }
        public DbSet<StoredMessage> Messages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Matches = Set<Match>();
            Boards = Set<SerializedBoard>();
            Friendships = Set<Friendship>();
            Requests = Set<FriendRequest>();
            Messages = Set<StoredMessage>();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SerializedConnect4Board>()
                .HasBaseType<SerializedBoard>();

            builder.Entity<SerializedChessBoard>()
                .HasBaseType<SerializedBoard>();

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Player1Matches)
                .WithOne(m => m.Player1);

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Player2Matches)
                .WithOne(m => m.Player2);

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.User1Friendships)
                .WithOne(f => f.User1);

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.User2Friendships)
                .WithOne(f => f.User2);

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.SentRequests)
                .WithOne(r => r.Sender);

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.ReceivedRequests)
                .WithOne(r => r.Receiver);

            builder.Entity<Match>()
                .HasKey(m => m.Id);

            builder.Entity<Match>()
                .HasMany(m => m.Messages)
                .WithOne(m => m.Match);

            builder.Entity<Match>()
                .HasOne(m => m.Board)
                .WithOne(b => b.Match)
                .HasForeignKey<SerializedBoard>(b => b.MatchId);

            base.OnModelCreating(builder);
        }
    }
}
