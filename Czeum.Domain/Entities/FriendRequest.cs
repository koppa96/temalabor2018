using Czeum.Abstractions.Domain;

namespace Czeum.Domain.Entities
{
    public class FriendRequest : EntityBase
    {
        public ApplicationUser Sender { get; set; }
        public ApplicationUser Receiver { get; set; }
    }
}