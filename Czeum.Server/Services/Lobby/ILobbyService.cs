using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;

namespace Czeum.Server.Services.Lobby
{
    public interface ILobbyService
    {
        LobbyData AddLobby(LobbyData lobbyData);
        bool JoinPlayerToLobby(string player, int lobbyId);
        void DisconnectPlayerFromLobby(string player, int lobbyId);
        void InvitePlayerToLobby(int lobbyId, string player);
        void CancelInviteFromLobby(int lobbyId, string player);
        string KickGuest(int lobbyId);
        LobbyData FindUserLobby(string user);
        List<LobbyData> GetLobbies();
        void UpdateLobbySettings(ref LobbyData lobbyData);
        LobbyData GetLobby(int lobbyId);
        bool ValidateModifier(string modifier, int lobbyId);
        bool LobbyExists(int lobbyId);
    }
}
