using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Logging;
using Prism.Windows.Navigation;
using Czeum.Core.ClientCallbacks;
using Czeum.Core.DTOs.Abstractions.Lobbies;
using Czeum.Core.Services;
using Czeum.Client.Interfaces;
using Czeum.Core.Enums;
using Czeum.Core.DTOs.Wrappers;
using Flurl.Http;
using Czeum.Core.DTOs.Extensions;

namespace Czeum.Client.ViewModels {
    public class LobbyPageViewModel : ViewModelBase
    {
        private ILobbyService lobbyService;
        private INavigationService navigationService;
        private ILoggerFacade loggerService;
        private IDialogService dialogService;
        private IUserManagerService userManagerService;
        private ILobbyClient lobbyClient;
        private IHubService hubService;
        private IMessageService messageService;
        private IMessageStore messageStore;
        public ILobbyStore lobbyStore { get; private set; }

        public ObservableCollection<LobbyData> LobbyList { get => lobbyStore.LobbyList; }


        private ObservableCollection<LobbyData> filteredList = new ObservableCollection<LobbyData>();
        public ObservableCollection<LobbyData> FilteredList {
            get => filteredList; 
            private set {
                filteredList = value;
                RaisePropertyChanged();
            }
        }
        public string Username { get => userManagerService.Username; }
        public string NameFilter { get; set; } = "";
        public bool HidePrivate { get; set; } = false;
        public bool FilterChess { get; set; } = false;
        public bool FilterConnect4 { get; set; } = false;

        public LobbyPageViewModel(ILobbyService lobbyService,
                                  INavigationService navigationService,
                                  ILoggerFacade loggerService,
                                  IDialogService dialogService,
                                  ILobbyClient lobbyClient,
                                  IUserManagerService userManagerService,
                                  IHubService hubService,
                                  ILobbyStore lobbyStore,
                                  IMessageService messageService,
                                  IMessageStore messageStore
                                
            ) {
            this.lobbyService = lobbyService;
            this.navigationService = navigationService;
            this.loggerService = loggerService;
            this.dialogService = dialogService;
            this.userManagerService = userManagerService;
            this.lobbyClient = lobbyClient;
            this.hubService = hubService;
            this.lobbyStore = lobbyStore;
            this.messageService = messageService;
            this.messageStore = messageStore;

            JoinLobbyCommand = new DelegateCommand<Guid?>(JoinLobby);
            CreateLobbyCommand = new DelegateCommand<string>(CreateLobby);
            ApplyFiltersCommand = new DelegateCommand(FilterLobbyList);
            ClearFiltersCommand = new DelegateCommand(ClearFilters);

            FilteredList = new ObservableCollection<LobbyData>(LobbyList.AsEnumerable());
            lobbyStore.LobbyList.CollectionChanged += (s, e) => FilterLobbyList();
        }


        public ICommand JoinLobbyCommand { get; }
        public ICommand CreateLobbyCommand { get; }
        public ICommand ApplyFiltersCommand { get; }
        public ICommand ClearFiltersCommand { get; }


        private void ClearFilters()
        {
            HidePrivate = false;
            NameFilter = "";
            FilterLobbyList();
        }

        private void FilterLobbyList()
        {
            FilteredList = new ObservableCollection<LobbyData>(LobbyList.AsEnumerable().Where(l => 
                (!HidePrivate || (l.Access != LobbyAccess.Private)) // Only perform filtering by visibility if HidePrivate is set
                &&
                (l.Name.Contains(NameFilter))
                &&
                (!(FilterChess || FilterConnect4) || // Only perform filtering if at least one type-filter is ticked, else ignore
                    (FilterChess && l.GetGameType() == GameType.Chess) || (FilterConnect4 &&l.GetGameType() == GameType.Connect4) // I am not proud of this
                )
            ));
        }
        private async void JoinLobby(Guid? id)
        {
            if (!id.HasValue)
            {
                return;
            }
            try
            {
                var lobby = await lobbyService.JoinToLobbyAsync(id.Value);
                lobbyStore.SelectedLobby = lobby.Content;
                await lobbyStore.UpdateLobby(lobby.Content);
                var messages = await messageService.GetMessagesOfLobbyAsync(id.Value);
                await messageStore.SetMessages(messages);
                navigationService.Navigate(PageTokens.LobbyDetails.ToString(), null);
            } 
            catch(FlurlHttpException e)
            {
                await dialogService.ShowError($"Could not connect to lobby, reason: {e.Message}");
            }

        }

        private async void CreateLobby(string lobbyTypeString)
        {
            var access = LobbyAccess.Public;
            string name = null;
            GameType? type;
            switch (lobbyTypeString)
            {
                case "Chess":
                    type = GameType.Chess;
                    break;
                case "Connect4":
                    type = GameType.Connect4;
                    break;
                default:
                    return;
            }
            var result = await lobbyService.CreateAndAddLobbyAsync(type.Value, access, name);
            await lobbyStore.AddLobby(result.Content);
            lobbyStore.SelectedLobby = result.Content;
            var messages = await messageService.GetMessagesOfLobbyAsync(result.Content.Id);
            await messageStore.SetMessages(messages);
            navigationService.Navigate(PageTokens.LobbyDetails.ToString(), null);
        }

        public bool IsUserInvited()
        {
            return true;
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);

            List<LobbyDataWrapper> lobbies = await lobbyService.GetLobbies();
            await lobbyStore.ClearLobbies();
            await lobbyStore.AddLobbies(lobbies.Select(x => x.Content));

            await hubService.ConnectToHubAsync();
        }
    }
}
