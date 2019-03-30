using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;

namespace Czeum.Server.Services.ServiceContainer
{
    public interface IServiceContainer
    {
        IGameService FindService(MoveData moveData);
        IGameService FindService(LobbyData lobbyData);
        IGameService GetRandomService();
    }
}