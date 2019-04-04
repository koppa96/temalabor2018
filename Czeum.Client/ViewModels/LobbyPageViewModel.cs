using Czeum.Abstractions.DTO;
using Czeum.Client.Interfaces;
using Czeum.DTO.Connect4;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Logging;
using Prism.Windows.Navigation;

namespace Czeum.Client.ViewModels {
    class LobbyPageViewModel : ViewModelBase
    {
        private ILobbyService lobbyService;
        private INavigationService navigationService;
        private ILoggerFacade loggerService;

        public ObservableCollection<LobbyData> LobbyList { get; private set; }

        public LobbyPageViewModel(ILobbyService lobbyService, INavigationService navigationService, ILoggerFacade loggerService) {
            this.lobbyService = lobbyService;
            this.navigationService = navigationService;
            this.loggerService = loggerService;

            this.lobbyService.QueryLobbyList();
            LobbyList = lobbyService.LobbyList;
            JoinLobbyCommand = new DelegateCommand<int?>(JoinLobby);
        }

        public ICommand JoinLobbyCommand { get; }

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
    }
}
