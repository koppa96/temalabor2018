using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connect4Dtos;
using System.Collections;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;

namespace Connect4Client
{
    class ConnectionManager {
        private static readonly ConnectionManager instance = new ConnectionManager();
        public static ConnectionManager Instance { get { return instance; } }
        private HubConnection hubConnection;
        public string UserName { get; set; }

        //static ConnectionManager() { }
        private ConnectionManager() { }

        public async Task CreateConnection() {
            hubConnection = new HubConnectionBuilder().WithUrl(App.AppUrl + "/gamehub", options => {
                options.AccessTokenProvider = () => Task.FromResult(App.Token);
            }).Build();

            hubConnection.On("CannotCreateLobbyFromOtherLobby", () => ShowDialog("Failed to create lobby", "You cannot create a lobby while you're inside another lobby."));
            hubConnection.On<LobbyData>("LobbyCreated", LobbyCreated);
            hubConnection.On<LobbyData>("LobbyAddedHandler", LobbyAddedHandler);
            hubConnection.On<MatchDto>("MatchCreated", MatchCreated);
            hubConnection.On<int>("LobbyDeleted", LobbyDeleted);
            hubConnection.On("NotEnoughPlayersHandler", () => ShowDialog("Failed to start the match", "There aren't enough players in the lobby to start the match."));
            hubConnection.On("CannotSetOtherLobby", () => ShowDialog("Failed to change lobby settings", "Only the host is allowed to change lobby settings"));
            hubConnection.On<int>("GetInvitationTo", GetInvitationTo);
            hubConnection.On("GuestDisconnected", () => ShowDialog("A player has disconnected", "The guest has left the lobby. The match cannot start without a second player present."));
            hubConnection.On("HostDisconnected", () => ShowDialog("A player has disconnected", "The host has left the lobby. You are the new host."));
            hubConnection.On<LobbyData>("JoinedToLobby", JoinedToLobby);
            hubConnection.On<string>("PlayerJoinedToLobby", PlayerJoinedToLobby);
            hubConnection.On("FailedToJoinLobby", () => ShowDialog("Failed to join lobby", "You cannot join this lobby as it is private and you are not invited."));
            //hubConnection.On("IncorrectMatchHandler", );
            hubConnection.On("ColumnFullHandler", () => ShowDialog("Failed to place item", "The selected column is full. Choose another column to place an item in."));
            //hubConnection.On("MatchFinishedHandler", );
            hubConnection.On("NotYourTurnHandler", () => ShowDialog("Failed to place item", "You can only place items on your turn."));
            hubConnection.On<MatchDto>("SuccessfulPlacement", SuccessfulPlacement);
            hubConnection.On<MatchDto>("SuccessfulEnemyPlacement", SuccessfulEnemyPlacement);
            //hubConnection.On<int, int>("EnemyVictoryHandler", );
            hubConnection.On("GuestKicked", () => ShowDialog("Guest kicked", "Your guest has been kicked."));
            hubConnection.On("YouHaveBeenKicked", YouHaveBeenKicked);
            hubConnection.On("OnlyHostCanInvite", () => ShowDialog("Failed to invite player", "Only the host of the lobby can invite players."));
            hubConnection.On("NobodyToKick", () => ShowDialog("Failed to kick guest", "There is no guest to be kicked."));
            hubConnection.On<LobbyData>("LobbyChanged", LobbyChanged);
            //hubConnection.On<LobbyData>("UserInvited", );
            hubConnection.On("CannotStartOtherMatch", () => ShowDialog("Failed to start match", "Only the host of the lobby can start the match"));
            hubConnection.On("InvalidLobbyId", () => ShowDialog("Invalid lobby ID", "The given lobby ID is invalid"));

            await hubConnection.StartAsync();
        }
        internal void PlaceItem(int matchId, int column) {
            hubConnection.InvokeAsync("PlaceItem", matchId, column);
        }

        internal void KickGuest(int lobbyId) {
            hubConnection.InvokeAsync("KickGuest", lobbyId);
        }

        internal void CancelInvitationOf(string selectedPlayer, int lobbyId) {
            hubConnection.InvokeAsync("CancelInvitationof", lobbyId, selectedPlayer);
        }

        internal void SendInvitationTo(string invitedPlayer, int lobbyId) {
            hubConnection.InvokeAsync("SendInvitationTo", lobbyId, invitedPlayer);
        }

        internal void ChangeLobbySettings(LobbyData lobby) {
            hubConnection.InvokeAsync("LobbySettingsChanged", lobby);
        }

        internal void StartMatch(LobbyData lobby) {
            hubConnection.InvokeAsync("CreateMatchAsync", lobby.LobbyId);
        }


        internal void CreateLobby(LobbyStatus status) {
            hubConnection.InvokeAsync("CreateLobby", status.ToString());
        }

        public void ConnectToLobby(int lobbyId) {
            hubConnection.InvokeAsync("JoinLobby", lobbyId);
        }

        public void DisconnectFromLobby(int lobbyId) {
            hubConnection.InvokeAsync("DisconnectFromLobby", lobbyId);
        }

        public async Task<IList<LobbyData>> GetLobbies(){
            return await hubConnection.InvokeAsync<List<LobbyData>>("GetLobbies");
        }

        public async Task<IList<MatchDto>> GetMatches() {
            return await hubConnection.InvokeAsync<List<MatchDto>>("GetMatches");
        }
        private async void ShowDialog(string errorTitle, string errorMessage) {
#pragma warning disable CS4014
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                ContentDialog errorDialog = new ContentDialog() {
                    Title = errorTitle,
                    Content = errorMessage,
                    CloseButtonText = "Ok",
                };

                errorDialog.ShowAsync();
            });
#pragma warning restore CS4014
        }

        public async Task CloseConnectionAsync() {
            await hubConnection.StopAsync();
            hubConnection = null;
        }

        // --- RMI Functions ---

        private void LobbyChanged(LobbyData lobby) {
            LobbyRepository.Instance.RefreshLobbySettings(lobby);
        }

        private void LobbyAddedHandler(LobbyData lobby) {
            LobbyRepository.Instance.AddItem(lobby);
        }

        private void LobbyDeleted(int lobbyId) {
            LobbyRepository.Instance.DeleteItemById(lobbyId);
        }

        public void PlayerJoinedToLobby(string player) {
            LobbyRepository.Instance.RefreshLobbyGuest(player);
        }

        public void LobbyCreated(LobbyData lobby) {
            LobbyRepository.Instance.LobbyCreated(lobby);
        }

        public void JoinedToLobby(LobbyData lobby) {
            LobbyRepository.Instance.JoinedToLobby(lobby);
        }
        
        public void MatchCreated(MatchDto match) {
            MatchRepository.Instance.AddItem(match);
            LobbyRepository.Instance.AfterMatchStarted();
        }

        public void YouHaveBeenKicked() {
            LobbyRepository.Instance.KickedFromLobby();
            ShowDialog("You have been kicked from the lobby!", "Ha, looser!!!");
        }

        private void GetInvitationTo(int lobbyId) {
#pragma warning disable CS4014
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => {
                string inviter = LobbyRepository.Instance.FindHostOf(lobbyId);
                ContentDialog inviteDialog = new ContentDialog {
                    Title = "You have been invited to a lobby",
                    Content = $"{inviter} has invited you to their lobby. Do you wish to join them?",
                    PrimaryButtonText = "Join",
                    CloseButtonText = "Cancel"
                };

                ContentDialogResult result = await inviteDialog.ShowAsync();
                if (result == ContentDialogResult.Primary) {
                    ConnectToLobby(lobbyId);
                }
                else {
                }
            });
#pragma warning restore CS4014
        }

        private void SuccessfulPlacement(MatchDto match) {
            MatchRepository.Instance.RefreshMatch(match);
        }

        private void SuccessfulEnemyPlacement(MatchDto match) {
            MatchRepository.Instance.RefreshMatch(match);
        }

    }
}
