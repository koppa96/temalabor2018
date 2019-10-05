using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.DTO;
using Czeum.DTO.Wrappers;

namespace Czeum.ClientCallback
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
        Task ReceiveLobbyMessage(Message message);
        Task ReceiveLobbyInvite(LobbyDataWrapper lobbyData);
    }
}