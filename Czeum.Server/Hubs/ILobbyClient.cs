using System.Threading.Tasks;
using Czeum.Abstractions.DTO;

namespace Czeum.Server.Hubs
{
    public interface ILobbyClient
    {
        Task LobbyDeleted(int lobbyId);
        Task LobbyCreated(LobbyData lobbyData);
        Task LobbyChanged(LobbyData lobbyData);
        Task JoinedToLobby(LobbyData lobbyData);
        Task KickedFromLobby();
    }
}