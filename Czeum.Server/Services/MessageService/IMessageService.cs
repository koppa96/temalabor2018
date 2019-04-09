using System.Collections.Generic;
using Czeum.DTO;

namespace Czeum.Server.Services.MessageService
{
    public interface IMessageService
    {
        Message SendToLobby(int lobbyId, string message, string sender);
        Message SendToMatch(int matchId, string message, string sender);
        List<Message> GetMessagesOfLobby(int lobbyId);
        List<Message> GetMessagesOfMatch(int matchId);
    }
}