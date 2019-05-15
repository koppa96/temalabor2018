using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Czeum.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public List<Match> Player1Matches { get; set; }
        public List<Match> Player2Matches { get; set; }

        public List<Friendship> User1Friendships { get; set; }
        public List<Friendship> User2Friendships { get; set; }
        
        public List<FriendRequest> SentRequests { get; set; }
        public List<FriendRequest> ReceivedRequests { get; set; }
    }
}
