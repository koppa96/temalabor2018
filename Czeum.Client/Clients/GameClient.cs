using Czeum.Client.Interfaces;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Czeum.Core.ClientCallbacks;
using Czeum.Core.DTOs;
using Czeum.Core.Services;

namespace Czeum.Client.Clients
{
    class GameClient : IGameClient
    {

        private IMatchService matchService;
        private IMatchStore matchStore;
        private IHubService hubService;
        private INavigationService navigationService;
        private IMessageService messageService;
        private IMessageStore messageStore;

        public GameClient(
            IMatchStore matchStore, 
            IHubService hubService, 
            INavigationService navigationService, 
            IMatchService matchService,
            IMessageService messageService,
            IMessageStore messageStore)
        {
            this.matchService = matchService;
            this.matchStore = matchStore;
            this.hubService = hubService;
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.messageStore = messageStore;

            hubService.Connection.On<MatchStatus>(nameof(ReceiveResult), ReceiveResult);
            hubService.Connection.On<MatchStatus>(nameof(MatchCreated), MatchCreated);
            hubService.Connection.On<Guid, Message>(nameof(ReceiveMatchMessage), ReceiveMatchMessage);
        }

        public async Task ReceiveResult(MatchStatus status)
        {
            await matchStore.UpdateMatch(status);
        }

        public async Task MatchCreated(MatchStatus status)
        {
            await matchStore.AddMatch(status);
            navigationService.Navigate(PageTokens.Match.ToString(), null);
        }

        public async Task ReceiveMatchMessage(Guid matchId, Message message)
        {
            // We are in the match
            if (matchStore.SelectedMatch.Id == matchId)
            {
                await messageStore.AddMessage(message);
            }
        }
    }
}
