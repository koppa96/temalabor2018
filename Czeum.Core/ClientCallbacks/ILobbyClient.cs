using System;
using System.Threading.Tasks;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Wrappers;

namespace Czeum.Core.ClientCallbacks
{
    /// <summary>
    /// An interface that the GameHub uses to notify its clients about lobby handling events.
    /// </summary>
    public interface ILobbyClient
    {
        Task LobbyDeleted(Guid lobbyId);
        Task LobbyAdded(LobbyDataWrapper lobbyData);
        Task LobbyChanged(LobbyDataWrapper lobbyData);
        Task KickedFromLobby();
        Task ReceiveLobbyMessage(Guid lobbyId, Message message);
        Task ReceiveLobbyInvite(LobbyDataWrapper lobbyData);
    }
}