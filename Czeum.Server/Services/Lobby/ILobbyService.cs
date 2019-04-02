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
        void AddLobby(LobbyData lobbyData);
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
        LobbyData CreateLobby(Type type);
        void AddMessage(int lobbyId, Message message);
        List<Message> GetMessages(int lobbyId);
        bool ValidateMessageSender(int lobbyId, string sender);
        string GetOtherPlayer(int lobbyId, string player);
    }
}
