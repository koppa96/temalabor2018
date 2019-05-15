using System;

namespace Czeum.DTO
{
    /// <summary>
    /// Represents a message that was sent to a lobby or a match.
    /// </summary>
    public class Message
    {
        public string Sender { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
    }
}