namespace Czeum.DAL.Entities
{
    public class FriendRequest : EntityBase
    {
        public ApplicationUser Sender { get; set; }
        public ApplicationUser Receiver { get; set; }
    }
}