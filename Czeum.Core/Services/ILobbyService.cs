using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Core.DTOs.Abstractions.Lobbies;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.Enums;

namespace Czeum.Core.Services
{
    public interface ILobbyService
    {
        Task<LobbyDataWrapper> JoinToLobbyAsync(Guid lobbyId);
        Task DisconnectFromCurrentLobbyAsync();
        Task DisconnectPlayerFromLobby(string username);
        Task<LobbyDataWrapper> InvitePlayerToLobby(Guid lobbyId, string player);
        Task<LobbyDataWrapper> CancelInviteFromLobby(Guid lobbyId, string player);
        Task<LobbyDataWrapper> KickGuestAsync(Guid lobbyId, string guestName);
        Task<LobbyData> GetLobbyOfUser(string user);
        Task<List<LobbyDataWrapper>> GetLobbies();
        Task<LobbyDataWrapper> UpdateLobbySettingsAsync(LobbyDataWrapper lobbyData);
        Task<LobbyDataWrapper> GetLobby(Guid lobbyId);
        Task<bool> LobbyExists(Guid lobbyId);
        Task<LobbyDataWrapper> CreateAndAddLobbyAsync(int gameIdentifier, LobbyAccess access, string name);
        Task<IEnumerable<string>> GetOthersInLobby(Guid lobbyId);
        Task<LobbyDataWrapper> GetCurrentLobbyAsync();
    }
}
