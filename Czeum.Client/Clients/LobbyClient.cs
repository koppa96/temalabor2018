using System;
using System.Threading.Tasks;
using Czeum.Core.DTOs;
using Czeum.Client.Interfaces;
using Czeum.Core.ClientCallbacks;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Windows.Navigation;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.Services;
using Windows.UI.Xaml.Controls;
using Flurl.Http;

namespace Czeum.Client.Clients {

    /// <summary>
    /// Provides the interface on which the remote hub can execute function calls. Most actions invoked by ILobbyService will cause a callback to arrive here.
    /// </summary>
    class LobbyClient : ILobbyClient
    {
        private ILobbyStore lobbyStore;
        private IHubService hubService;
        private ILobbyService lobbyService;
        private INavigationService navigationService;
        private IDialogService dialogService;

        public LobbyClient(ILobbyStore lobbyStore, 
                           ILobbyService lobbyService,
                           IHubService hubService, 
                           INavigationService navigationService, 
                           IDialogService dialogService)
        {
            this.lobbyStore = lobbyStore;
            this.hubService = hubService;
            this.lobbyService = lobbyService;
            this.navigationService = navigationService;
            this.dialogService = dialogService;

            hubService.Connection.On<Guid>(nameof(LobbyDeleted), LobbyDeleted);
            hubService.Connection.On<LobbyDataWrapper>(nameof(LobbyChanged), LobbyChanged);
            hubService.Connection.On(nameof(KickedFromLobby), KickedFromLobby);
            hubService.Connection.On<LobbyDataWrapper>(nameof(LobbyAdded), LobbyAdded);
            hubService.Connection.On<Guid, Message>(nameof(ReceiveLobbyMessage), ReceiveLobbyMessage);
            hubService.Connection.On<LobbyDataWrapper>(nameof(ReceiveLobbyInvite), ReceiveLobbyInvite);
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

        public async Task ReceiveLobbyInvite(LobbyDataWrapper lobbyData)
        {
            var result = await dialogService.ShowConfirmation($"You have been invited to a {lobbyData.GameType.ToString()} lobby by {lobbyData.Content.Host}. Do you want to join them?");
            if(result == ContentDialogResult.Primary)
            {
                try
                {
                    if (lobbyStore.SelectedLobby != null)
                    {
                        await lobbyService.DisconnectFromCurrentLobbyAsync();
                    }
                    var lobby = await lobbyService.JoinToLobbyAsync(lobbyData.Content.Id);
                    lobbyStore.SelectedLobby = lobby.Content;
                    await lobbyStore.UpdateLobby(lobby.Content);
                    navigationService.Navigate(PageTokens.LobbyDetails.ToString(), null);
                }
                catch (FlurlHttpException e)
                {
                    await dialogService.ShowError("Could not connect to the lobby");
                }
            }
            throw new NotImplementedException();
        }
    }
}
