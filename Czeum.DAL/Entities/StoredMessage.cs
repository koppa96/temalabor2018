using System;
using System.ComponentModel.DataAnnotations;
using Czeum.DTO;

namespace Czeum.DAL.Entities
{
    public class StoredMessage
    {
        [Key]
        public int MessageId { get; set; }

        public ApplicationUser Sender { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }

        public Match Match { get; set; }

        public Message ToMessage()
        {
            return new Message
            {
                Sender = Sender.UserName,
                Text = Text,
                Timestamp = Timestamp
            };
        }
    }
}