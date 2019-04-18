using Czeum.Abstractions.DTO;
using Czeum.Client.Interfaces;
using Czeum.DTO.Connect4;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Prism.Commands;
using Prism.Logging;
using Prism.Windows.Navigation;
using Czeum.ClientCallback;
using Windows.ApplicationModel.Core;

namespace Czeum.Client.ViewModels {
    class LobbyPageViewModel : ViewModelBase
    {
        private ILobbyService lobbyService;
        private INavigationService navigationService;
        private ILoggerFacade loggerService;
        private IDialogService dialogService;
        private IUserManagerService userManagerService;
        private ILobbyClient lobbyClient;
        private IHubService hubService;
        public ILobbyStore lobbyStore { get; private set; }

        public ObservableCollection<LobbyData> LobbyList { get => lobbyService.LobbyList; }

        public LobbyPageViewModel(ILobbyService lobbyService, INavigationService navigationService, ILoggerFacade loggerService, IDialogService dialogService, 
            ILobbyClient lobbyClient, IUserManagerService userManagerService, IHubService hubService, ILobbyStore lobbyStore) {
            this.lobbyService = lobbyService;
            this.navigationService = navigationService;
            this.loggerService = loggerService;
            this.dialogService = dialogService;
            this.userManagerService = userManagerService;
            this.lobbyClient = lobbyClient;
            this.hubService = hubService;
            this.lobbyStore = lobbyStore;

            lobbyService.QueryLobbyList();
            JoinLobbyCommand = new DelegateCommand<int?>(JoinLobby);
            CreateLobbyCommand = new DelegateCommand<Type>(CreateLobby);
        }

        public ICommand JoinLobbyCommand { get; }
        public ICommand CreateLobbyCommand { get; }

        private void JoinLobby(int? index)
        {
            if (!index.HasValue)
            {
                loggerService.Log("Tried navigating to an invalid Lobby ID", Category.Debug, Priority.None);
                return;
            }
            lobbyService.JoinLobby(index.Value);
            loggerService.Log($"Navigating to Lobby #{index.Value}", Category.Debug, Priority.None);
            navigationService.Navigate("LobbyDetails", null);
        }

        private async void CreateLobby(Type lobbyType)
        {
            if (lobbyType == null)
            {
                lobbyType = typeof(Connect4LobbyData);
            }
            await lobbyService.CreateLobby(lobbyType);
        }

        public override async void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            if (e.NavigationMode != NavigationMode.Back)
            {
                //Let normal navigation pass as usual
                base.OnNavigatingFrom(e, viewModelState, suspending);
                return;
            }
            //Intercept back navigation
            e.Cancel = true;
            var result = await dialogService.ShowConfirmation("Are you sure you want to log out?");
            if (result == ContentDialogResult.Primary)
            {
                //Perform logout
                await userManagerService.LogOutAsync();
                await hubService.DisconnectFromHub();
                navigationService.Navigate("Login", null);
            }
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            await hubService.ConnectToHubAsync();
        }
    }
}
