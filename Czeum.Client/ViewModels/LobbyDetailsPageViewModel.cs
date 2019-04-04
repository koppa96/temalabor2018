using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.Client.Interfaces;
using Prism.Logging;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

namespace Czeum.Client.ViewModels {
    class LobbyDetailsPageViewModel : ViewModelBase{
        private ILobbyService lobbyService;
        private ILoggerFacade loggerService;
        private INavigationService navigationService;

        public LobbyData SelectedLobby  => lobbyService.CurrentLobby;

        public LobbyDetailsPageViewModel(INavigationService navigationService, ILoggerFacade loggerService,
            ILobbyService lobbyService)
        {
            this.lobbyService = lobbyService;
            this.navigationService = navigationService;
            this.loggerService = loggerService;
        }


    }
}
