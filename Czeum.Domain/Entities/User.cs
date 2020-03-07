using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Czeum.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public List<UserMatch> Matches { get; set; }
        public List<Match> WonMatches { get; set; }

        public List<Friendship> User1Friendships { get; set; }
        public List<Friendship> User2Friendships { get; set; }
        
        public List<FriendRequest> SentRequests { get; set; }
        public List<FriendRequest> ReceivedRequests { get; set; }

        public List<Notification> ReceivedNotifications { get; set; }
    }
}
