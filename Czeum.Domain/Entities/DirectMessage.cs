using Czeum.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Domain.Entities
{
    public class DirectMessage : EntityBase
    {
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }

        public Friendship Friendship { get; set; }
        public Guid? FriendshipId { get; set; }

        public User Sender { get; set; }
        public Guid? SenderId { get; set; }
    }
}
