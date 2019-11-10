using Czeum.Client.Interfaces;
using Czeum.Core.ClientCallbacks;
using Czeum.Core.DTOs;
using Prism.Commands;
using Prism.Logging;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Czeum.Client.ViewModels
{
    public class MatchPageViewModel : ViewModelBase
    {
        private IMatchService matchService;
        private INavigationService navigationService;
        private ILoggerFacade loggerService;
        private IDialogService dialogService;
        private IUserManagerService userManagerService;
        private IGameClient gameClient;
        private IHubService hubService;
        public IMatchStore MatchStore { get; private set; }

        public ObservableCollection<MatchStatus> LobbyList { get => matchService.MatchList; }

        public ICommand OpenGameCommand { get; set; }

        public MatchPageViewModel(IMatchService matchService, INavigationService navigationService, ILoggerFacade loggerService, IDialogService dialogService,
            IGameClient gameClient, IUserManagerService userManagerService, IHubService hubService, IMatchStore matchStore)
        {
            this.matchService = matchService;
            this.navigationService = navigationService;
            this.loggerService = loggerService;
            this.dialogService = dialogService;
            this.userManagerService = userManagerService;
            this.gameClient = gameClient;
            this.hubService = hubService;
            this.MatchStore = matchStore;

            matchService.QueryMatchList();

            OpenGameCommand = new DelegateCommand<MatchStatus>(OpenGame);
        }

        private void OpenGame(MatchStatus match)
        {
            matchService.OpenMatch(match);
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            await hubService.ConnectToHubAsync();
        }
    }
}
