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
        private INavigationService navigationService;
        private ILobbyStore lobbyStore;
        private IHubService hubService;

        //    public ObservableCollection<LobbyData> LobbyList { get => lobbyStore.LobbyList; }
        //    public LobbyData CurrentLobby { get => lobbyStore.SelectedLobby; }

        public LobbyService(INavigationService navigationService, ILobbyStore lobbyStore, IHubService hubService, IUserManagerService userManagerService)
        {
            this.userManagerService = userManagerService;
            this.navigationService = navigationService;
            this.lobbyStore = lobbyStore;
            this.hubService = hubService;
        }

        //    public async Task JoinLobby(int index)
        //    {
        //        await hubService.Connection.InvokeAsync("JoinLobby", index);
        //    }

        //    public async Task LeaveLobby()
        //    {
        //        await hubService.Connection.InvokeAsync("DisconnectFromlobby", CurrentLobby.Id);
        //    }

        //    public async Task InvitePlayer(string player)
        //    {
        //        hubService.Connection.InvokeAsync("InvitePlayer", CurrentLobby.Id, player);
        //    }

        //    public async Task CreateLobby(Type type)
        //    {
        //        await hubService.Connection.InvokeAsync("CreateLobby", type, LobbyAccess.Public, "...");
        //    }

        //    public async Task UpdateLobby(LobbyData lobbyData)
        //    {
        //        await hubService.Connection.InvokeAsync("UpdateLobby", CurrentLobby);
        //    }


        //    public async Task QueryLobbyList()
        //    {
        //        HttpClientHandler ignoreCertHandler = new HttpClientHandler();
        //        ignoreCertHandler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        //        using (var client = new HttpClient(ignoreCertHandler))
        //        {
        //            try
        //            {
        //                var targetUrl = Flurl.Url.Combine(BASE_URL, "/api/game/lobbies");
        //                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", userManagerService.AccessToken);
        //                var response = await client.GetAsync(targetUrl);

        //                var lobbies = JsonConvert.DeserializeObject<List<LobbyData>>(await response.Content.ReadAsStringAsync(),
        //                    new JsonSerializerSettings()
        //                    {
        //                        TypeNameHandling = TypeNameHandling.All
        //                    });
        //                await lobbyStore.ClearLobbies();
        //                foreach (var lobby in lobbies)
        //                {
        //                    await lobbyStore.AddLobby(lobby);
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                //TODO
        //            }

        //        }
        //    }

        //    public async Task CreateMatch()
        //    {
        //        await hubService.Connection.InvokeAsync("CreateMatch", CurrentLobby.Id);

        //    }

        //    public async Task KickGuest()
        //    {
        //        await hubService.Connection.InvokeAsync("KickGuest", CurrentLobby.Id);
        //    }
        //}
        public Task<LobbyDataWrapper> CancelInviteFromLobby(Guid lobbyId, string player)
        {
            throw new NotImplementedException();
        }
        public Task DisconnectPlayerFromLobby(string username)
        {
            throw new NotImplementedException();
        }

        public Task<LobbyDataWrapper> CreateAndAddLobbyAsync(GameType type, LobbyAccess access, string name)
        {
            var createDTO = new CreateLobbyDto()
            {
                GameType = type,
                LobbyAccess = access,
                Name = name
            };
            return BASE_URL.AppendPathSegment("lobbies").PostJsonAsync(createDTO).ReceiveJson<LobbyDataWrapper>();
        }

        public Task DisconnectFromCurrentLobbyAsync()
        {
            return BASE_URL.AppendPathSegment("lobbies/current/leave").PostAsync(null);
        }


        public Task<List<LobbyDataWrapper>> GetLobbies()
        {
            return BASE_URL.AppendPathSegment("lobbies").GetJsonAsync<List<LobbyDataWrapper>>();
        }

        public Task<LobbyDataWrapper> GetLobby(Guid lobbyId)
        {
            //return BASE_URL.AppendPathSegment("lobbies").
            throw new NotImplementedException();
        }

        public Task<LobbyData> GetLobbyOfUser(string user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetOthersInLobby(Guid lobbyId)
        {
            throw new NotImplementedException();
        }

        public Task<LobbyDataWrapper> InvitePlayerToLobby(Guid lobbyId, string player)
        {
            return BASE_URL.AppendPathSegment($"{lobbyId}/invite").SetQueryParam("playerName", player).PostJsonAsync(null).ReceiveJson<LobbyDataWrapper>();
        }

        public Task<LobbyDataWrapper> JoinToLobbyAsync(Guid lobbyId)
        {
            return BASE_URL.AppendPathSegment($"{lobbyId}/join").PostJsonAsync(null).ReceiveJson<LobbyDataWrapper>();
        }

        public Task<LobbyDataWrapper> KickGuestAsync(Guid lobbyId, string guestName)
        {
            return BASE_URL.AppendPathSegment($"lobbies/{lobbyId}/kick/{guestName}").PostJsonAsync(null).ReceiveJson<LobbyDataWrapper>();
        }

        public Task<bool> LobbyExists(Guid lobbyId)
        {
            throw new NotImplementedException();
        }

        public void RemoveLobby(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<LobbyDataWrapper> UpdateLobbySettingsAsync(LobbyDataWrapper lobbyData)
        {
            return BASE_URL.AppendPathSegment($"lobbies/{lobbyData.Content.Id}").PutJsonAsync(lobbyData).ReceiveJson<LobbyDataWrapper>();
        }
    }
}