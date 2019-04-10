using Czeum.Abstractions.DTO;

namespace Czeum.Client.Interfaces
{
    public interface ITypeDispatcher
    {
        ILobbyRenderer DispatchLobbyRenderer(LobbyData lobbyData);
    }
}