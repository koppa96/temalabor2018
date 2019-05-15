namespace Czeum.DAL.Entities
{
    public class FriendRequest
    {
        public int RequestId { get; set; }
        
        public ApplicationUser Sender { get; set; }
        public ApplicationUser Receiver { get; set; }
    }
}