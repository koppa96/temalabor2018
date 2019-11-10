using Czeum.Client.Interfaces;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Abstractions;
using Czeum.Core.DTOs.Chess;
using Czeum.Core.DTOs.Connect4;
using Czeum.Core.Services;
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
    class MatchService : Core.Services.IMatchService
    {
        //private string BASE_URL = "https://localhost:44301";
        private string BASE_URL = App.Current.Resources["BaseUrl"].ToString();
        private IUserManagerService userManagerService;
        private IMatchStore matchStore;
        private IHubService hubService;
        private INavigationService navigationService;
        private Core.Services.IMatchService matchService;

        public MatchService(IMatchStore matchStore, IHubService hubService, IUserManagerService userManagerService, INavigationService navigationService)
        {
            this.matchStore = matchStore;
            this.hubService = hubService;
            this.userManagerService = userManagerService;
            this.navigationService = navigationService;
        }

        public ObservableCollection<MatchStatus> MatchList { get => matchStore.MatchList; }

        public MatchStatus CurrentMatch { get => matchStore.SelectedMatch; }

        public async Task DoMove(MoveData moveData)
        {
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

        public Task<MatchStatus> CreateMatchAsync(Guid lobbyId)
        {
            throw new NotImplementedException();
        }

        public Task CreateRandomMatchAsync(IEnumerable<string> players)
        {
            throw new NotImplementedException();
        }

        public Task<MatchStatus> HandleMoveAsync(MoveData moveData)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MatchStatus>> GetMatchesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetOthersInMatchAsync(Guid matchId)
        {
            throw new NotImplementedException();
        }
    }
}
