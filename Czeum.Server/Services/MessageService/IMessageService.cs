using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.DTO;

namespace Czeum.Server.Services.MessageService
{
    public interface IMessageService
    {
        Message SendToLobby(int lobbyId, string message, string sender);
        Task<Message> SendToMatchAsync(int matchId, string message, string sender);
        List<Message> GetMessagesOfLobby(int lobbyId);
        Task<List<Message>> GetMessagesOfMatchAsync(int matchId);
    }
}