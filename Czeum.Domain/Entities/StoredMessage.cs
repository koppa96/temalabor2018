using System;
using Czeum.Core.Domain;

namespace Czeum.Domain.Entities
{
    public class StoredMessage : EntityBase
    {
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }

        public User Sender { get; set; }
        public Guid? SenderId { get; set; }
        public Match Match { get; set; }
        public Guid? MatchId { get; set; }
    }
}