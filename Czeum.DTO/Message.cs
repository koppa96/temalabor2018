using System;

namespace Czeum.DTO
{
    public class Message
    {
        public string Sender { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
    }
}