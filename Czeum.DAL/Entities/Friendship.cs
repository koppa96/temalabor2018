namespace Czeum.DAL.Entities
{
    public class Friendship : EntityBase
    {
        public ApplicationUser User1 { get; set; }
        public ApplicationUser User2 { get; set; }
    }
}
