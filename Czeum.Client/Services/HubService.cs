using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.Client.Interfaces;
using Prism.Windows.Navigation;

namespace Czeum.Client.Services {
    class HubService : IHubService {
        private INavigationService navigationService;
        private IDialogService dialogService;
        private ILobbyService lobbyService;

        public HubService(INavigationService navigationService, IDialogService dialogService,
            ILobbyService lobbyService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.lobbyService = lobbyService;
        }


    }
}
