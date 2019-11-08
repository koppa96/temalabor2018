using Czeum.Client.Interfaces;
using Czeum.Core.ClientCallbacks;
using Czeum.Core.DTOs;
using Czeum.Core.Enums;
using Czeum.Core.Services;
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

        public string Username => userManagerService.Username;

        public bool FilterChess { get; set; } = false;
        public bool FilterConnect4{ get; set; } = false;
        public bool FilterYour{ get; set; } = false;
        public bool FilterOpponent{ get; set; } = false;
        public bool FilterWon{ get; set; } = false;
        public bool FilterLost{ get; set; } = false;
        public bool FilterDraw{ get; set; } = false;

        public ObservableCollection<MatchStatus> MatchList { get => MatchStore.MatchList; }
        private ObservableCollection<MatchStatus> filteredList = new ObservableCollection<MatchStatus>();
        public ObservableCollection<MatchStatus> FilteredList {
            get => filteredList;
            private set {   
                filteredList = value;
                RaisePropertyChanged();
            }
        }

        public ICommand OpenGameCommand { get; set; }
        public ICommand ApplyFiltersCommand { get; }
        public ICommand ClearFiltersCommand { get; }

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

            OpenGameCommand = new DelegateCommand<MatchStatus>(OpenGame);
            ApplyFiltersCommand = new DelegateCommand(FilterMatchList);
            ClearFiltersCommand = new DelegateCommand(ClearFilters);

            FilteredList = new ObservableCollection<MatchStatus>(MatchList.AsEnumerable());
            matchStore.MatchList.CollectionChanged += (s, e) => FilterMatchList();
        }

        private void ClearFilters()
        {
            FilterChess = false;
            FilterConnect4 = false;
            FilterYour = false;
            FilterOpponent = false;
            FilterWon = false;
            FilterLost = false;
            FilterDraw = false;
        }

        private void FilterMatchList()
        {
            FilteredList = new ObservableCollection<MatchStatus>(MatchList.AsEnumerable().Where(m =>
                (!(FilterChess || FilterConnect4) // Only perform filtering for type if at least one type-filter is ticked, else ignore
                    || (FilterChess && m.CurrentBoard.GameType == GameType.Chess)           // Check for chess if we filter for it
                    || (FilterConnect4 && m.CurrentBoard.GameType == GameType.Connect4)     // Check for C4 if we filter for it
                )
                &&
                (!(FilterYour || FilterOpponent || FilterWon || FilterLost || FilterDraw) // Only perform filtering for state if at least one state-filter is ticked, else ignore
                    || (FilterYour && m.State == GameState.YourTurn)        // Check for your turns if we filter for it
                    || (FilterOpponent && m.State == GameState.EnemyTurn)   // Check for opponent's turns if we filter for it
                    || (FilterWon && m.State == GameState.Won)              // Check for won matches if we filter for it
                    || (FilterLost && m.State == GameState.Lost)            // Check for lost matches if we filter for it
                    || (FilterDraw && m.State == GameState.Draw)            // Check for tied matches if we filter for it
                )
            ));
        }

        private void OpenGame(MatchStatus match)
        {
            MatchStore.SelectMatch(match);
            //PageTokens targetPage = match.CurrentBoard.GetPageToken();
            //navigationService.Navigate(targetPage.ToString(), null);
            navigationService.Navigate("MatchDetails", null);
        }


        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);

            IEnumerable<MatchStatus> matches = await matchService.GetMatchesAsync();
            await MatchStore.ClearMatches();
            await MatchStore.AddMatches(matches);

            await hubService.ConnectToHubAsync();
        }
    }
}
