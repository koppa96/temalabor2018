namespace Czeum.DAL.Entities
{
    public class Friendship
    {
        public int FriendshipId { get; set; }
        public ApplicationUser User1 { get; set; }
        public ApplicationUser User2 { get; set; }
    }
}
