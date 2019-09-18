using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.Client.Interfaces;
using Czeum.DTO;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Prism.Windows.Navigation;

namespace Czeum.Client.Services {

    /// <summary>
    /// Service interfacing towards the remote hub. ViewModel calls into ILobbyService which in turn calls the corresponding function through the hub
    /// TODO: register functions through IHubService instead of constantly accessing it's property.
    /// </summary>
    class LobbyService : ILobbyService
    {
        //private string BASE_URL = "https://localhost:44301";
        private string BASE_URL = App.Current.Resources["BaseUrl"].ToString();
        private IUserManagerService userManagerService;
        private INavigationService navigationService;
        private ILobbyStore lobbyStore;
        private IHubService hubService;

        public ObservableCollection<LobbyData> LobbyList { get => lobbyStore.LobbyList; }
        public LobbyData CurrentLobby { get => lobbyStore.SelectedLobby; }

        public LobbyService(INavigationService navigationService, ILobbyStore lobbyStore, IHubService hubService, IUserManagerService userManagerService)
        {
            this.userManagerService = userManagerService;
            this.navigationService = navigationService;
            this.lobbyStore = lobbyStore;
            this.hubService = hubService;
        }

        public async Task JoinLobby(int index)
        {
            await hubService.Connection.InvokeAsync("JoinLobby", index);
        }

        public async Task LeaveLobby()
        {
            await hubService.Connection.InvokeAsync("DisconnectFromlobby", CurrentLobby.LobbyId);
        }

        public async Task InvitePlayer(string player)
        {
            hubService.Connection.InvokeAsync("InvitePlayer", CurrentLobby.LobbyId, player);
        }

        public async Task CreateLobby(Type type)
        {
            await hubService.Connection.InvokeAsync("CreateLobby", type, LobbyAccess.Public, "...");
        }

        public async Task UpdateLobby(LobbyData lobbyData)
        {
            await hubService.Connection.InvokeAsync("UpdateLobby", CurrentLobby);
        }

        
        public async Task QueryLobbyList()
        {
            HttpClientHandler ignoreCertHandler = new HttpClientHandler();
            ignoreCertHandler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            using (var client = new HttpClient(ignoreCertHandler))
            {
                try
                {
                    var targetUrl = Flurl.Url.Combine(BASE_URL, "/api/game/lobbies");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", userManagerService.AccessToken);
                    var response = await client.GetAsync(targetUrl);

                    var lobbies = JsonConvert.DeserializeObject<List<LobbyData>>(await response.Content.ReadAsStringAsync(),
                        new JsonSerializerSettings()
                        {
                            TypeNameHandling = TypeNameHandling.All
                        });
                    await lobbyStore.ClearLobbies();
                    foreach (var lobby in lobbies)
                    {
                        await lobbyStore.AddLobby(lobby);
                    }
                }
                catch (Exception e)
                {
                    //TODO
                }

            }
        }

        public async Task CreateMatch()
        {
            await hubService.Connection.InvokeAsync("CreateMatch", CurrentLobby.LobbyId);

        }

        public async Task KickGuest()
        {
            await hubService.Connection.InvokeAsync("KickGuest", CurrentLobby.LobbyId);
        }
    }
}
