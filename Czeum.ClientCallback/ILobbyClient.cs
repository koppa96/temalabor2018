using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.DTO;

namespace Czeum.ClientCallback
{
    /// <summary>
    /// An interface that the GameHub uses to notify its clients about lobby handling events.
    /// </summary>
    public interface ILobbyClient
    {
        Task LobbyDeleted(int lobbyId);
        Task LobbyCreated(LobbyData lobbyData);
        Task LobbyAdded(LobbyData lobbyData);
        Task LobbyChanged(LobbyData lobbyData);
        Task JoinedToLobby(LobbyData lobbyData, List<Message> messages);
        Task KickedFromLobby();
        Task LobbyMessageSent(Message message);
        Task ReceiveLobbyMessage(Message message);
    }
}