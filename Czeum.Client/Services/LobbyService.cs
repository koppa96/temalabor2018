using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Czeum.Core.DTOs;
using Czeum.Client.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Prism.Windows.Navigation;
using Czeum.Core.DTOs.Abstractions.Lobbies;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.Enums;
using Windows.UI.Xaml;
using Flurl;
using Flurl.Http;
using Czeum.Core.DTOs.Lobbies;
using Czeum.Core.Services;

namespace Czeum.Client.Services
{

    /// <summary>
    /// Service interfacing towards the remote hub. ViewModel calls into ILobbyService which in turn calls the corresponding function through the hub
    /// TODO: register functions through IHubService instead of constantly accessing it's property.
    /// </summary>
    class LobbyService : Czeum.Core.Services.ILobbyService
    {
        private string BASE_URL = App.Current.Resources["BaseUrl"].ToString();
        private IUserManagerService userManagerService;

        public LobbyService(IUserManagerService userManagerService)
        {
            this.userManagerService = userManagerService;
        }
               
        public Task<LobbyDataWrapper> CreateAndAddLobbyAsync(GameType type, LobbyAccess access, string name)
        {
            var createDTO = new CreateLobbyDto()
            {
                GameType = type,
                LobbyAccess = access,
                Name = name
            };
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment("api/lobbies").PostJsonAsync(createDTO).ReceiveJson<LobbyDataWrapper>();
        }

        public Task DisconnectFromCurrentLobbyAsync()
        {
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment("api/lobbies/current/leave").PostAsync(null);
        }


        public Task<List<LobbyDataWrapper>> GetLobbies()
        {
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment("api/lobbies").GetJsonAsync<List<LobbyDataWrapper>>();
        }

        public Task<LobbyDataWrapper> InvitePlayerToLobby(Guid lobbyId, string player)
        {
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment($"api/lobbies/{lobbyId}/invite").SetQueryParam("playerName", player).PostJsonAsync(null).ReceiveJson<LobbyDataWrapper>();
        }

        public Task<LobbyDataWrapper> JoinToLobbyAsync(Guid lobbyId)
        {
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment($"api/lobbies/{lobbyId}/join").PostJsonAsync(null).ReceiveJson<LobbyDataWrapper>();
        }

        public Task<LobbyDataWrapper> KickGuestAsync(Guid lobbyId, string guestName)
        {
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment($"api/lobbies/{lobbyId}/kick/{guestName}").PostJsonAsync(null).ReceiveJson<LobbyDataWrapper>();
        }

        public Task<LobbyDataWrapper> UpdateLobbySettingsAsync(LobbyDataWrapper lobbyData)
        {
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment($"api/lobbies/{lobbyData.Content.Id}").PutJsonAsync(lobbyData).ReceiveJson<LobbyDataWrapper>();
        }

        // Not needed on the client
        public Task<bool> LobbyExists(Guid lobbyId)
        {
            throw new NotImplementedException();
        }

        // Not needed on the client
        public void RemoveLobby(Guid id)
        {
            throw new NotImplementedException();
        }

        // Not needed on the client
        public Task DisconnectPlayerFromLobby(string username)
        {
            throw new NotImplementedException();
        }

        // Not needed on the client
        public Task<LobbyDataWrapper> GetLobby(Guid lobbyId)
        {
            throw new NotImplementedException();
        }

        // Not needed on the client
        public Task<LobbyData> GetLobbyOfUser(string user)
        {
            throw new NotImplementedException();
        }

        // Not needed on the client
        public Task<IEnumerable<string>> GetOthersInLobby(Guid lobbyId)
        {
            throw new NotImplementedException();
        }

        public Task<LobbyDataWrapper> CancelInviteFromLobby(Guid lobbyId, string player)
        {
            throw new NotImplementedException();
        }
    }
}