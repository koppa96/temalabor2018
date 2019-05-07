using Czeum.Abstractions.DTO;
using Czeum.Client.Interfaces;
using Czeum.DTO;
using Czeum.DTO.Chess;
using Czeum.DTO.Connect4;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Prism.Windows.Navigation;
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
        private INavigationService navigationService;

        public MatchService(IMatchStore matchStore, IHubService hubService, IUserManagerService userManagerService, INavigationService navigationService)
        {
            this.matchStore = matchStore;
            this.hubService = hubService;
            this.userManagerService = userManagerService;
            this.navigationService = navigationService;
        }

        public ObservableCollection<MatchStatus> MatchList { get => matchStore.MatchList; }

        public MatchStatus CurrentMatch { get => matchStore.SelectedMatch; }

        public async Task DoMove(int column)
        {
            var moveData = new Connect4MoveData() { MatchId = CurrentMatch.MatchId, Column = column };
            await hubService.Connection.InvokeAsync("ReceiveMove", moveData);
        }

        public void OpenMatch(MatchStatus match)
        {
            matchStore.SelectMatch(match);
            PageTokens targetPage = GetPageToken(match);
            navigationService.Navigate(targetPage.ToString(), null);
        }

        //TODO: Extract to another service
        private PageTokens GetPageToken(MatchStatus match)
        {
            if(match.CurrentBoard.GetType() == typeof(ChessMoveResult))
            {
                return PageTokens.Chess;
            }
            if(match.CurrentBoard.GetType() == typeof(Connect4MoveResult))
            {
                return PageTokens.Connect4;
            }
            return PageTokens.Match;
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
