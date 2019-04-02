using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.DTO;

namespace Czeum.ClientCallback
{
    public interface ILobbyClient
    {
        Task LobbyDeleted(int lobbyId);
        Task LobbyCreated(LobbyData lobbyData);
        Task LobbyChanged(LobbyData lobbyData);
        Task JoinedToLobby(LobbyData lobbyData, List<Message> messages);
        Task KickedFromLobby();
        Task ReceiveLobbyMessage(Message message);
    }
}