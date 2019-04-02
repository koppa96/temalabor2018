using System.Collections.Generic;
using Czeum.Abstractions.DTO;
using Czeum.DTO;

namespace Czeum.Server.Services.Lobby
{
    public interface ILobbyStorage
    {
        IEnumerable<LobbyData> GetLobbies();
        LobbyData GetLobby(int lobbyId);
        void AddLobby(LobbyData lobbyData);
        void RemoveLobby(int lobbyId);
        void UpdateLobby(LobbyData lobbyData);
        LobbyData GetLobbyOfUser(string user);
        void AddMessage(int lobbyId, Message message);
        List<Message> GetMessages(int lobbyId);
    }
}