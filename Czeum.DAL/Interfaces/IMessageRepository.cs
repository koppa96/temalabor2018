using System.Collections.Generic;
using Czeum.DTO;

namespace Czeum.DAL.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(int matchId, Message message);
        List<Message> GetMessagesForMatch(int matchId);
    }
}