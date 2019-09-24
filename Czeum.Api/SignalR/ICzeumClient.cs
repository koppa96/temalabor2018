using Czeum.ClientCallback;

namespace Czeum.Api.SignalR
{
    public interface ICzeumClient : IGameClient, ILobbyClient, IErrorClient, IFriendClient
    {
    }
}
