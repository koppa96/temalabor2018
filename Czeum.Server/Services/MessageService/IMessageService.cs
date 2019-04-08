using System.Collections.Generic;
using Czeum.DTO;

namespace Czeum.Server.Services.MessageService
{
    public interface IMessageService
    {
        bool SendToLobby(int lobbyId, Message message, string sender);
        bool SendToMatch(int matchId, Message message, string sender);
        List<Message> GetMessagesOfLobby(int lobbyId);
        List<Message> GetMessagesOfMatch(int matchId);
    }
}