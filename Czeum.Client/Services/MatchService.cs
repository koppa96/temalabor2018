using Czeum.Client.Interfaces;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Abstractions;
using Czeum.Core.DTOs.Chess;
using Czeum.Core.DTOs.Connect4;
using Czeum.Core.DTOs.Extensions;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.Services;
using Flurl.Http;
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
        //private string BASE_URL = "https://localhost:44301";
        private string BASE_URL = App.Current.Resources["BaseUrl"].ToString();
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

        public Task<MatchStatus> CreateMatchAsync(Guid lobbyId)
        {
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment($"api/matches").SetQueryParam("lobbyId", lobbyId).PostJsonAsync(null).ReceiveJson<MatchStatus>();
        }


        public Task<MatchStatus> HandleMoveAsync(MoveData moveData)
        {
            var wrapper = new MoveDataWrapper()
            {
                Content = moveData,
                GameType = moveData.GetGameType()
            };
            try
            {
                return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment($"api/matches/moves").PutJsonAsync(wrapper).ReceiveJson<MatchStatus>();
            } 
            catch(FlurlHttpException e)
            {
                // TODO
            }
            return Task.FromResult<MatchStatus>(null);

        }

        public Task<IEnumerable<MatchStatus>> GetMatchesAsync()
        {
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment("api/matches").GetJsonAsync<IEnumerable<MatchStatus>>();
        }


        // Not needed on the client
        public Task<IEnumerable<string>> GetOthersInMatchAsync(Guid matchId)
        {
            throw new NotImplementedException();
        }

        // Not needed on the client
        public Task CreateRandomMatchAsync(IEnumerable<string> players)
        {
            throw new NotImplementedException();
        }
    }
}
