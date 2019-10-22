using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.Core.DTOs;
using Czeum.Client.Interfaces;
using Czeum.Core.ClientCallbacks;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Windows.Navigation;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Czeum.Core.DTOs.Abstractions.Lobbies;
using Czeum.Core.DTOs.Wrappers;

namespace Czeum.Client.Clients {

    /// <summary>
    /// Provides the interface on which the remote hub can execute function calls. Most actions invoked by ILobbyService will cause a callback to arrive here.
    /// </summary>
    class LobbyClient : ILobbyClient
    {
        private Core.Services.ILobbyService lobbyService;
        private ILobbyStore lobbyStore;
        private IHubService hubService;
        private INavigationService navigationService;
        
        public LobbyClient(ILobbyStore lobbyStore, IHubService hubService, INavigationService navigationService, Core.Services.ILobbyService lobbyService)
        {
            this.lobbyService = lobbyService;
            this.lobbyStore = lobbyStore;
            this.hubService = hubService;
            this.navigationService = navigationService;

            hubService.CreateHubConnection();
            hubService.Connection.On<Guid>(nameof(LobbyDeleted), LobbyDeleted);
            hubService.Connection.On<LobbyDataWrapper>(nameof(LobbyChanged), LobbyChanged);
            hubService.Connection.On(nameof(KickedFromLobby), KickedFromLobby);
            hubService.Connection.On<LobbyDataWrapper>(nameof(LobbyAdded), LobbyAdded);
        }

        public async Task LobbyDeleted(Guid lobbyId)
        {
            if(lobbyStore.SelectedLobby?.Id == lobbyId)
            {
                navigationService.Navigate(PageTokens.Lobby.ToString(), null);
                navigationService.ClearHistory();
            }
            await lobbyStore.RemoveLobby(lobbyId);
        }

        public async Task LobbyChanged(LobbyDataWrapper lobbyData)
        {
            await lobbyStore.UpdateLobby(lobbyData.Content);
        }

        public async Task KickedFromLobby()
        {
            navigationService.Navigate(PageTokens.Lobby.ToString(), null);
            navigationService.ClearHistory();
            lobbyStore.SelectedLobby = null;
        }

        public async Task LobbyAdded(LobbyDataWrapper lobbyData)
        {
            await lobbyStore.AddLobby(lobbyData.Content);
        }

        public Task ReceiveLobbyMessage(Guid lobbyId, Message message)
        {
            throw new NotImplementedException();
        }

        public Task ReceiveLobbyInvite(LobbyDataWrapper lobbyData)
        {
            throw new NotImplementedException();
        }
    }
}
