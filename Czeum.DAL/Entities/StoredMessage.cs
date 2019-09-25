using System;
using Czeum.DTO;

namespace Czeum.DAL.Entities
{
    public class StoredMessage : EntityBase
    {
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }

        public ApplicationUser Sender { get; set; }
        public Match Match { get; set; }
    }
}