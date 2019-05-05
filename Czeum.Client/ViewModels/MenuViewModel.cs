using Czeum.Client.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Czeum.Client.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private const string CurrentPageTokenKey = "CurrentPageToken";
        private Dictionary<PageTokens, bool> _canNavigateLookup;
        private PageTokens _currentPageToken;
        private INavigationService _navigationService;
        private IDialogService _dialogService;
        private ISessionStateService _sessionStateService;
        private IHubService _hubService;
        private IUserManagerService _userManagerService;

        public MenuViewModel(IEventAggregator eventAggregator, INavigationService navigationService, ISessionStateService sessionStateService, 
            IDialogService dialogService, IUserManagerService userManagerService, IHubService hubService)
        {
            eventAggregator.GetEvent<NavigationStateChangedEvent>().Subscribe(OnNavigationStateChanged);
            _navigationService = navigationService;
            _dialogService = dialogService;
            _sessionStateService = sessionStateService;
            _hubService = hubService;
            _userManagerService = userManagerService;


            Commands = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel { DisplayName = "Lobbies", SymbolName = "\ue787", Command = new DelegateCommand(() => NavigateToPageAsync(PageTokens.Lobby), () => CanNavigateToPage(PageTokens.Lobby)) },
                new MenuItemViewModel { DisplayName = "Matches", SymbolName = "\ue7fc", Command = new DelegateCommand(() => NavigateToPageAsync(PageTokens.Match), () => CanNavigateToPage(PageTokens.Match)) },
                new MenuItemViewModel { DisplayName = "Log out", SymbolName = "\ue711", Command = new DelegateCommand(() => NavigateToPageAsync(PageTokens.Login), () => CanNavigateToPage(PageTokens.Login)) }
            };

            _canNavigateLookup = new Dictionary<PageTokens, bool>();

            foreach (PageTokens pageToken in Enum.GetValues(typeof(PageTokens)))
            {
                _canNavigateLookup.Add(pageToken, true);
            }

            if (_sessionStateService.SessionState.ContainsKey(CurrentPageTokenKey))
            {
                // Resuming, so update the menu to reflect the current page correctly
                PageTokens currentPageToken;
                if (Enum.TryParse(_sessionStateService.SessionState[CurrentPageTokenKey].ToString(), out currentPageToken))
                {
                    UpdateCanNavigateLookup(currentPageToken);
                    RaiseCanExecuteChanged();
                }
            }
        }

        public ObservableCollection<MenuItemViewModel> Commands { get; set; }

        private void OnNavigationStateChanged(NavigationStateChangedEventArgs args)
        {
            if (_currentPageToken == PageTokens.Login)
            {
                foreach (var key in _canNavigateLookup.Keys.ToList())
                {
                    _canNavigateLookup[key] = true;
                    RaiseCanExecuteChanged();
                }
            }

            PageTokens currentPageToken;
            if (Enum.TryParse(args.Sender.Content.GetType().Name.Replace("Page", string.Empty), out currentPageToken))
            {
                _sessionStateService.SessionState[CurrentPageTokenKey] = currentPageToken.ToString();
                UpdateCanNavigateLookup(currentPageToken);
                RaiseCanExecuteChanged();
            }
        }

        private async Task NavigateToPageAsync(PageTokens pageToken)
        {
            if (CanNavigateToPage(pageToken))
            {
                if(pageToken == PageTokens.Login)
                {
                    var result = await _dialogService.ShowConfirmation("Are you sure you want to log out?");
                    if (result != ContentDialogResult.Primary)
                    {
                        return;
                    }
                    //Perform logout
                    await _userManagerService.LogOutAsync();
                    await _hubService.DisconnectFromHub();
                }

                if (_navigationService.Navigate(pageToken.ToString(), null))
                {
                    UpdateCanNavigateLookup(pageToken);
                    RaiseCanExecuteChanged();
                }
            }
        }

        private bool CanNavigateToPage(PageTokens pageToken)
        {
            return _canNavigateLookup[pageToken];
        }

        private void UpdateCanNavigateLookup(PageTokens navigatedTo)
        {
            if(navigatedTo == PageTokens.Login)
            {
                foreach (var key in _canNavigateLookup.Keys.ToList())
                {
                    _canNavigateLookup[key] = false;
                }
                _currentPageToken = navigatedTo;
                return;
            }
            _canNavigateLookup[_currentPageToken] = true;
            _canNavigateLookup[navigatedTo] = false;
            _currentPageToken = navigatedTo;
        }

        private void RaiseCanExecuteChanged()
        {
            foreach (var item in Commands)
            {
                (item.Command as DelegateCommand).RaiseCanExecuteChanged();
            }
        }
    }
}
