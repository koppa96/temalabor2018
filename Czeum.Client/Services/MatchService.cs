using Czeum.Abstractions.DTO;
using Czeum.Client.Interfaces;
using Czeum.DTO;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Client.Services
{
    class MatchService : IMatchService
    {
        private string BASE_URL = "https://localhost:44301";
        private IUserManagerService userManagerService;
        private IMatchStore matchStore;
        private IHubService hubService;

        public MatchService(IMatchStore matchStore, IHubService hubService, IUserManagerService userManagerService)
        {
            this.matchStore = matchStore;
            this.hubService = hubService;
            this.userManagerService = userManagerService;
        }

        public ObservableCollection<MatchStatus> MatchList { get => matchStore.MatchList; }

        public MatchStatus CurrentMatch { get => matchStore.SelectedMatch; }

        public async Task DoMove(MoveData moveData)
        {
            await hubService.Connection.InvokeAsync("DoMove", moveData);
        }

        public async Task QueryMatchList()
        {
            HttpClientHandler ignoreCertHandler = new HttpClientHandler();
            ignoreCertHandler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            using (var client = new HttpClient(ignoreCertHandler))
            {
                try
                {
                    var targetUrl = Flurl.Url.Combine(BASE_URL, "/api/game/matches");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", userManagerService.AccessToken);
                    var response = await client.GetAsync(targetUrl);

                    var matches = JsonConvert.DeserializeObject<List<MatchStatus>>(await response.Content.ReadAsStringAsync(),
                        new JsonSerializerSettings()
                        {
                            TypeNameHandling = TypeNameHandling.All
                        });
                    await matchStore.ClearMatches();
                    foreach (var match in matches)
                    {
                        await matchStore.AddMatch(match);
                    }
                }
                catch (Exception e)
                {
                    //TODO
                }

            }
        }
    }
}
