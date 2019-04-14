using Czeum.DAL.Entities;
using Czeum.DTO;

namespace Czeum.DAL.Extensions
{
    public static class StoredMessageExtensions
    {
        public static Message ToMessage(this StoredMessage message)
        {
            return new Message
            {
                Sender = message.Sender.UserName,
                Text = message.Text,
                Timestamp = message.Timestamp
            };
        }
    }
}
