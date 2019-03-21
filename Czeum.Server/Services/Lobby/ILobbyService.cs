using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.DTO.Lobby;
using Czeum.Server.Models.Lobby;

namespace Czeum.Server.Services.Lobby
{
    public interface ILobbyService
    {
        LobbyModel CreateLobby(string host, LobbyAccess access);
        bool JoinPlayerToLobby(string player, int lobbyId);
        void DisconnectPlayerFromLobby(string player, int lobbyId);
        void DeleteLobby(int lobbyId);
        void InvitePlayerToLobby(int lobbyId, string player);
        string KickGuest(int lobbyId);
        List<LobbyData> GetLobbyData();
        LobbyModel FindLobbyById(int lobbyId);
        LobbyModel FindUserLobby(string user);
    }
}
