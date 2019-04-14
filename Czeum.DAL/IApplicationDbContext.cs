using System.Threading;
using System.Threading.Tasks;
using Czeum.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Czeum.DAL
{
    public interface IApplicationDbContext
    {
        DbSet<Match> Matches { get; set; }
        DbSet<SerializedBoard> Boards { get; set; }
        DbSet<Friendship> Friendships { get; set; }
        DbSet<FriendRequest> Requests { get; set; }
        DbSet<StoredMessage> Messages { get; set; }
        DbSet<ApplicationUser> Users { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}