using System.ComponentModel.DataAnnotations;

namespace Czeum.DAL.Entities
{
    public class FriendRequest
    {
        [Key]
        public int RequestId { get; set; }
        
        public ApplicationUser Sender { get; set; }
        public ApplicationUser Receiver { get; set; }
    }
}