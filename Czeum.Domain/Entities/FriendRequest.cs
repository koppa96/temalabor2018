using System;
using Czeum.Core.Domain;

namespace Czeum.Domain.Entities
{
    public class FriendRequest : EntityBase
    {
        public User Sender { get; set; }
        public Guid? SenderId { get; set; }
        public User Receiver { get; set; }
        public Guid? ReceiverId { get; set; }
    }
}