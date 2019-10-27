using Czeum.Client.Interfaces;
using Czeum.Client.Views;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.Enums;
using Prism.Commands;
using Prism.Logging;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Czeum.Core.DTOs.Extensions;
using Czeum.Core.Services;
using Flurl.Http;

namespace Czeum.Client.ViewModels
{
    public class LobbyDetailsPageViewModel : ViewModelBase
    {
        private ILobbyService lobbyService;
        private ILoggerFacade loggerService;
        private INavigationService navigationService;
        private IUserManagerService userManagerService;
        private IMatchService matchService;
        private IMatchStore matchStore;
        private IDialogService dialogService;

        private string inviteeName;

        public string InviteeName {
            get => inviteeName;
            set => SetProperty(ref inviteeName, value);
        }

        public ILobbyStore lobbyStore { get; private set; }
        public ICommand SaveSettingsCommand { get; private set; }
        public ICommand CreateMatchCommand { get; private set; }
        public ICommand VisibilityChangeCommand { get; private set; }   
        public ICommand LeaveLobbyCommand{ get; private set; }
        public ICommand KickGuestCommand { get; private set; }
        public ICommand InvitePlayerCommand { get; private set; }

        public bool IsUserGuest => lobbyStore.SelectedLobby.Guests.Contains(userManagerService.Username);

        public LobbyDetailsPageViewModel(INavigationService navigationService, 
                                         ILoggerFacade loggerService,
                                         ILobbyService lobbyService, 
                                         IUserManagerService userManagerService, 
                                         ILobbyStore lobbyStore, 
                                         IMatchService matchService, 
                                         IMatchStore matchStore, 
                                         IDialogService dialogService)
        {
            this.lobbyService = lobbyService;
            this.navigationService = navigationService;
            this.loggerService = loggerService;
            this.userManagerService = userManagerService;
            this.lobbyStore = lobbyStore;
            this.matchService = matchService;
            this.matchStore = matchStore;
            this.dialogService = dialogService;

            SaveSettingsCommand = new DelegateCommand(SaveLobbySettings);
            CreateMatchCommand = new DelegateCommand(CreateMatch);
            VisibilityChangeCommand = new DelegateCommand<string>((s) => SetLobbyVisibility(s));
            InvitePlayerCommand = new DelegateCommand(InvitePlayer);
            KickGuestCommand = new DelegateCommand(KickGuest);
            LeaveLobbyCommand = new DelegateCommand(Leave);
        }

        private void Leave()
        {
            navigationService.Navigate(PageTokens.Lobby.ToString(), null);
            navigationService.ClearHistory();
        }

        private async void KickGuest()
        {
            // Client currently supports 2 player games so the only guest is the first one
            if(lobbyStore.SelectedLobby.Guests.Count < 1)
            {
                return;
            }
            var updatedLobby = await lobbyService.KickGuestAsync(lobbyStore.SelectedLobby.Id, lobbyStore.SelectedLobby.Guests[0]);
            await lobbyStore.UpdateLobby(updatedLobby.Content);
        }

        private async void InvitePlayer()
        {
            try
            {
                var updatedLobby = await lobbyService.InvitePlayerToLobby(lobbyStore.SelectedLobby.Id, InviteeName);
                await lobbyStore.UpdateLobby(updatedLobby.Content);
                InviteeName = "";
            } 
            catch (FlurlHttpException e)
            {
                await dialogService.ShowError("Could not invite player.");
            }
        }

        private void SetLobbyVisibility(string accessString)
        {
            LobbyAccess access = (LobbyAccess)Enum.Parse(typeof(LobbyAccess), accessString);
            lobbyStore.SelectedLobby.Access = access;
        }

        private async void CreateMatch()
        {
            try
            {
                var match = await matchService.CreateMatchAsync(lobbyStore.SelectedLobby.Id);
                await matchStore.AddMatch(match);
                navigationService.Navigate(PageTokens.Match.ToString(), null);
                navigationService.ClearHistory();
            }
            catch (FlurlHttpException e)
            {
                await dialogService.ShowError("Could not start the match");
            }
        }

        private async void SaveLobbySettings()
        {

            var wrapper = new LobbyDataWrapper()
            {
                GameType = lobbyStore.SelectedLobby.GetGameType(),
                Content = lobbyStore.SelectedLobby
            };
            try
            {
                var updatedLobby = await lobbyService.UpdateLobbySettingsAsync(wrapper);
                await lobbyStore.UpdateLobby(updatedLobby.Content);
            }
            catch (FlurlHttpException e)
            {
                await dialogService.ShowError("Could not save lobby settings");
            }
        }

        public async override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            try
            {
                await lobbyService.DisconnectFromCurrentLobbyAsync();
                lobbyStore.SelectedLobby = null;
            }
            catch (FlurlHttpException ex)
            {
                // We are not in a lobby anymore, nothing to do
            }
            base.OnNavigatingFrom(e, viewModelState, suspending);
        }
    }
}
