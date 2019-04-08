using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.DTO;

namespace Czeum.Server.Services.Lobby
{
    public interface ILobbyService
    {
        bool JoinPlayerToLobby(string player, int lobbyId);
        void DisconnectPlayerFromLobby(string player, int lobbyId);
        void InvitePlayerToLobby(int lobbyId, string player);
        void CancelInviteFromLobby(int lobbyId, string player);
        string KickGuest(int lobbyId);
        LobbyData FindUserLobby(string user);
        List<LobbyData> GetLobbies();
        void UpdateLobbySettings(LobbyData lobbyData);
        LobbyData GetLobby(int lobbyId);
        bool ValidateModifier(int lobbyId, string modifier);
        bool LobbyExists(int lobbyId);
        LobbyData CreateAndAddLobby(Type type, string host, LobbyAccess access, string name);
        string GetOtherPlayer(int lobbyId, string player);
    }
}
