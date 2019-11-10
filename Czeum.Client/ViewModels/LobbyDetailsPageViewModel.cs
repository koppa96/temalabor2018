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

namespace Czeum.Client.ViewModels
{
    public class LobbyDetailsPageViewModel : ViewModelBase
    {
        private Core.Services.ILobbyService lobbyService;
        private ILoggerFacade loggerService;
        private INavigationService navigationService;
        private IUserManagerService userManagerService;
        private Core.Services.IMatchService matchService;

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

        public LobbyDetailsPageViewModel(INavigationService navigationService, ILoggerFacade loggerService,
            Core.Services.ILobbyService lobbyService, IUserManagerService userManagerService, ILobbyStore lobbyStore, Core.Services.IMatchService matchService)
        {
            this.lobbyService = lobbyService;
            this.navigationService = navigationService;
            this.loggerService = loggerService;
            this.userManagerService = userManagerService;
            this.lobbyStore = lobbyStore;
            this.matchService = matchService;

            SaveSettingsCommand = new DelegateCommand(SaveLobbySettings);
            CreateMatchCommand = new DelegateCommand(CreateMatch);
            VisibilityChangeCommand = new DelegateCommand<string>((s) => SetLobbyVisibility(s));
            InvitePlayerCommand = new DelegateCommand(InvitePlayer);
            KickGuestCommand = new DelegateCommand(KickGuest);
            LeaveLobbyCommand = new DelegateCommand(Leave);
        }

        private async void Leave()
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
            await lobbyService.KickGuestAsync(lobbyStore.SelectedLobby.Id, lobbyStore.SelectedLobby.Guests[0]);
        }

        private async void InvitePlayer()
        {
            await lobbyService.InvitePlayerToLobby(lobbyStore.SelectedLobby.Id, InviteeName);
            InviteeName = "";
        }

        private void SetLobbyVisibility(string accessString)
        {
            LobbyAccess access = (LobbyAccess)Enum.Parse(typeof(LobbyAccess), accessString);
            lobbyStore.SelectedLobby.Access = access;
        }

        private async void CreateMatch()
        {
            await matchService.CreateMatchAsync(lobbyStore.SelectedLobby.Id);
        }

        private async void SaveLobbySettings()
        {

            var wrapper = new LobbyDataWrapper()
            {
                GameType = lobbyStore.SelectedLobby.GetGameType(),
                Content = lobbyStore.SelectedLobby
            };
            await lobbyService.UpdateLobbySettingsAsync(wrapper);
        }

        public async override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        { 
            await lobbyService.DisconnectFromCurrentLobbyAsync();
            base.OnNavigatingFrom(e, viewModelState, suspending);
        }
    }
}
