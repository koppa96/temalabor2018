using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.Client.Interfaces;
using Czeum.ClientCallback;
using Czeum.DTO;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Windows.Navigation;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Czeum.Abstractions.DTO.Lobbies;

namespace Czeum.Client.Clients {

    /// <summary>
    /// Provides the interface on which the remote hub can execute function calls. Most actions invoked by ILobbyService will cause a callback to arrive here.
    /// </summary>
    class LobbyClient : ILobbyClient
    {
        private ILobbyService lobbyService;
        private ILobbyStore lobbyStore;
        private IHubService hubService;
        private INavigationService navigationService;
        
        public LobbyClient(ILobbyStore lobbyStore, IHubService hubService, INavigationService navigationService, ILobbyService lobbyService)
        {
            this.lobbyService = lobbyService;
            this.lobbyStore = lobbyStore;
            this.hubService = hubService;
            this.navigationService = navigationService;

            hubService.CreateHubConnection();
            hubService.Connection.On<int>(nameof(LobbyDeleted), LobbyDeleted);
            hubService.Connection.On<LobbyData>(nameof(LobbyAdded), LobbyAdded);
            hubService.Connection.On<LobbyData>(nameof(LobbyCreated), LobbyCreated);
            hubService.Connection.On<LobbyData>(nameof(LobbyChanged), LobbyChanged);
            hubService.Connection.On<LobbyData, List<Message>>(nameof(JoinedToLobby), JoinedToLobby);
            hubService.Connection.On(nameof(KickedFromLobby), KickedFromLobby);
        }

        public async Task LobbyDeleted(int lobbyId)
        {
            if(lobbyService.CurrentLobby?.Id == lobbyId)
            {
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    navigationService.Navigate(PageTokens.Lobby.ToString(), null);
                    navigationService.ClearHistory();
                });
            }
            await lobbyStore.RemoveLobby(lobbyId);
        }

        public async Task LobbyCreated(LobbyData lobbyData)
        {
            lobbyStore.SelectedLobby = lobbyData;
            await lobbyStore.AddLobby(lobbyData);
            await JoinedToLobby(lobbyData, new List<Message>());
        }

        public async Task LobbyChanged(LobbyData lobbyData)
        {
            await lobbyStore.UpdateLobby(lobbyData);
        }

        public async Task JoinedToLobby(LobbyData lobbyData, List<Message> messages)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                lobbyStore.SelectedLobby = lobbyData;
                navigationService.Navigate(PageTokens.LobbyDetails.ToString(), null);
            });
        }

        public async Task KickedFromLobby()
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                navigationService.Navigate(PageTokens.Lobby.ToString(), null);
                navigationService.ClearHistory();
                lobbyStore.SelectedLobby = null;
            });
        }

        public async Task LobbyMessageSent(Message message)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveLobbyMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public async Task LobbyAdded(LobbyData lobbyData)
        {
            await lobbyStore.AddLobby(lobbyData);
        }
    }
}
