using System.Collections.Generic;
using Czeum.Abstractions.DTO;

namespace Czeum.Server.Services.Lobby
{
    public interface ILobbyStorage
    {
        IEnumerable<LobbyData> GetLobbies();
        LobbyData GetLobby(int lobbyId);
        void AddLobby(ref LobbyData lobbyData);
        void RemoveLobby(int lobbyId);
        void UpdateLobby(LobbyData lobbyData);
        LobbyData GetLobbyOfUser(string user);
    }
}