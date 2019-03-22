using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Czeum.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [InverseProperty("Player1")]
        public List<Match> Player1Matches { get; set; }

        [InverseProperty("Player2")]
        public List<Match> Player2Matches { get; set; }

        [InverseProperty("User1")]
        public List<Friendship> User1Friendships { get; set; }

        [InverseProperty("User2")]
        public List<Friendship> User2Friendships { get; set; }
        
        [InverseProperty("Sender")]
        public List<FriendRequest> SentRequests { get; set; }
        
        [InverseProperty("Receiver")] 
        public List<FriendRequest> Type { get; set; }
    }
}
