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
                .HasKey(m => m.MatchId);

            builder.Entity<Match>()
                .HasMany(m => m.Messages)
                .WithOne(m => m.Match);

            builder.Entity<Match>()
                .HasOne(m => m.Board)
                .WithOne(b => b.Match);

            builder.Entity<SerializedBoard>()
                .HasKey(b => b.BoardId);

            builder.Entity<StoredMessage>()
                .HasKey(m => m.MessageId);

            builder.Entity<FriendRequest>()
                .HasKey(r => r.RequestId);

            builder.Entity<Friendship>()
                .HasKey(f => f.FriendshipId);

            base.OnModelCreating(builder);
        }
    }
}
