using Czeum.Client.Interfaces;
using Czeum.ClientCallback;
using Czeum.DTO;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Czeum.Client.Clients
{
    class GameClient : IGameClient
    {

        private IMatchService matchService;
        private IMatchStore matchStore;
        private IHubService hubService;
        private INavigationService navigationService;

        public GameClient(IMatchStore matchStore, IHubService hubService, INavigationService navigationService, IMatchService matchService)
        {
            this.matchService = matchService;
            this.matchStore = matchStore;
            this.hubService = hubService;
            this.navigationService = navigationService;

            hubService.Connection.On<MatchStatus>(nameof(ReceiveResult), ReceiveResult);
            hubService.Connection.On<MatchStatus>(nameof(MatchCreated), MatchCreated);
            hubService.Connection.On<int, Message>(nameof(MatchMessageSent), MatchMessageSent);
            hubService.Connection.On<int, Message>(nameof(ReceiveMatchMessage), ReceiveMatchMessage);
        }

        public async Task ReceiveResult(MatchStatus status)
        {
            await matchStore.UpdateMatch(status);
        }

        public async Task MatchCreated(MatchStatus status)
        {
            await matchStore.AddMatch(status);
        }

        public async Task MatchMessageSent(int matchId, Message message)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveMatchMessage(int matchId, Message message)
        {
            throw new NotImplementedException();
        }
    }
}
