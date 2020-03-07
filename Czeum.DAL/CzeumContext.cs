using System;
using System.Reflection;
using Czeum.Core.Domain;
using Czeum.DAL.Seed;
using Czeum.Domain.Entities;
using Czeum.Domain.Entities.Achivements;
using Czeum.Domain.Entities.Boards;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Czeum.DAL
{
    public sealed class CzeumContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<Match> Matches { get; set; }
        public DbSet<SerializedBoard> Boards { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<FriendRequest> Requests { get; set; }
        public DbSet<StoredMessage> MatchMessages { get; set; }
        public DbSet<UserMatch> UserMatches { get; set; }
        public DbSet<DirectMessage> DirectMessages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Achivement> Achivements { get; set; }
        public DbSet<UserAchivement> UserAchivements { get; set; }

        public CzeumContext(DbContextOptions<CzeumContext> options)
            : base(options)
        {
            Matches = Set<Match>();
            Boards = Set<SerializedBoard>();
            Friendships = Set<Friendship>();
            Requests = Set<FriendRequest>();
            MatchMessages = Set<StoredMessage>();
            UserMatches = Set<UserMatch>();
            DirectMessages = Set<DirectMessage>();
            Notifications = Set<Notification>();
            Achivements = Set<Achivement>();
            UserAchivements = Set<UserAchivement>();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Entity<DoMovesAchivement>().HasData(AchivementSeed.DoMovesAchivements);
            builder.Entity<HaveWinRateAchivement>().HasData(AchivementSeed.HaveWinRateAchivements);
            builder.Entity<WinChessMatchesAchivement>().HasData(AchivementSeed.WinChessMatchesAchivements);
            builder.Entity<WinConnect4MatchesAchivement>().HasData(AchivementSeed.WinConnect4MatchesAchivements);
            builder.Entity<WinMatchesAchivement>().HasData(AchivementSeed.WinMatchesAchivements);
            builder.Entity<WinQuickMatchesAchivement>().HasData(AchivementSeed.WinQuickMatchesAchivements);

            // Achivement inheritance
            builder.Entity<DoMovesAchivement>().HasBaseType<Achivement>();
            builder.Entity<HaveWinRateAchivement>().HasBaseType<Achivement>();
            builder.Entity<WinChessMatchesAchivement>().HasBaseType<Achivement>();
            builder.Entity<WinConnect4MatchesAchivement>().HasBaseType<Achivement>();
            builder.Entity<WinMatchesAchivement>().HasBaseType<Achivement>();
            builder.Entity<WinQuickMatchesAchivement>().HasBaseType<Achivement>();
            
            base.OnModelCreating(builder);
        }
    }
}
