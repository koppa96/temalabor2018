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
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.DTO.Chess;

namespace Czeum.Client.ViewModels {
    public class LobbyPageViewModel : ViewModelBase
    {
        private ILobbyService lobbyService;
        private INavigationService navigationService;
        private ILoggerFacade loggerService;
        private IDialogService dialogService;
        private IUserManagerService userManagerService;
        private ILobbyClient lobbyClient;
        private IErrorClient errorClient;
        private IHubService hubService;
        public ILobbyStore lobbyStore { get; private set; }

        public ObservableCollection<LobbyData> LobbyList { get => lobbyService.LobbyList; }
        public string Username { get => userManagerService.Username; }

        public LobbyPageViewModel(ILobbyService lobbyService, INavigationService navigationService, ILoggerFacade loggerService, IDialogService dialogService, 
            ILobbyClient lobbyClient, IUserManagerService userManagerService, IHubService hubService, ILobbyStore lobbyStore, IErrorClient errorClient) {
            this.lobbyService = lobbyService;
            this.navigationService = navigationService;
            this.loggerService = loggerService;
            this.dialogService = dialogService;
            this.userManagerService = userManagerService;
            this.lobbyClient = lobbyClient;
            this.hubService = hubService;
            this.lobbyStore = lobbyStore;
            this.errorClient = errorClient;

            lobbyService.QueryLobbyList();
            JoinLobbyCommand = new DelegateCommand<int?>(JoinLobby);
            CreateLobbyCommand = new DelegateCommand<string>(CreateLobby);
        }

        public ICommand JoinLobbyCommand { get; }
        public ICommand CreateLobbyCommand { get; }

        private void JoinLobby(int? index)
        {
            if (!index.HasValue)
            {
                return;
            }
            lobbyService.JoinLobby(index.Value);
        }

        private async void CreateLobby(string lobbyTypeString)
        {
            switch (lobbyTypeString)
            {
                case "Chess":
                    await lobbyService.CreateLobby(typeof(ChessLobbyData));
                    break;
                case "Connect4":
                    await lobbyService.CreateLobby(typeof(Connect4LobbyData));
                    break;
                default:
                    break;
            }
        }

        public bool IsUserInvited()
        {
            return true;
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            await hubService.ConnectToHubAsync();
        }
    }
}
