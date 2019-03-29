using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connect4Dtos;
using System.Collections;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Czeum.Client.DTOs;

namespace Czeum.Client {
    class LobbyRepository {
        public static LobbyRepository Instance { get; } = new LobbyRepository();

        private static object lockObject = new object();
        private LobbyBrowserPage homePage;
        private LobbyPage lobbyPage;
        private ObservableCollection<LobbyData> lobbyList;
        public ObservableCollection<LobbyData> LobbyList { get { return lobbyList; } }
        private NotifyingLobbyData joinedLobby;
        public NotifyingLobbyData JoinedLobby { get { return joinedLobby; } set { joinedLobby = value; } }
        public LobbyData SelectedLobby{ get; set; }

        private LobbyRepository() {
        }

        public void LoadItems(ICollection<LobbyData> lobbies) {
            lobbyList = new ObservableCollection<LobbyData>(lobbies);
        }

        public void AddHomePage(LobbyBrowserPage page) {
            homePage = page;
        }

        public void AddLobbyPage(LobbyPage page) {
            lobbyPage = page;
        }

        public void LeaveLobby() {
            JoinedLobby = null;
        }

#pragma warning disable CS4014
        public void AddItem(LobbyData lobby) {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                if(lobbyList.Count(x => lobby.LobbyId == x.LobbyId) == 0) {
                    lobbyList.Add(lobby);   
                }
            });
        }

        public void AfterMatchStarted() {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                lobbyPage?.SuccessfulMatchStart();
            });
        }
        
        public void KickedFromLobby() {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                LeaveLobby();
                lobbyPage.Frame.GoBack();
            });
        }

        public void DeleteItemById(int lobbyId) {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                lobbyList.Remove(lobbyList.SingleOrDefault(x => x.LobbyId == lobbyId));
            });
        }

        public void RefreshLobbyGuest(string guestName){
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                joinedLobby.Guest = guestName;
            });
        }

        public void LobbyCreated(LobbyData lobby) {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                lobbyList.Add(lobby);
                joinedLobby = new NotifyingLobbyData(lobby);
                homePage?.SuccessfulLobbyJoin();
            });
        }

        public void JoinedToLobby(LobbyData lobby) {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                var lobbyInList = lobbyList.SingleOrDefault(x => x.LobbyId == lobby.LobbyId);
                lobbyList[lobbyList.IndexOf(lobbyInList)] = lobby;
                joinedLobby = new NotifyingLobbyData(lobby);
                homePage?.SuccessfulLobbyJoin();
            });
        }

        internal void RefreshLobbySettings(LobbyData lobby) {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                var lobbyInList = lobbyList.SingleOrDefault(x => x.LobbyId == lobby.LobbyId);
                lobbyList[lobbyList.IndexOf(lobbyInList)] = lobby;

                
                if(JoinedLobby?.LobbyId == lobby.LobbyId) {
                    JoinedLobby.InnerLobby = lobby;
                }
            });
        }

        internal string FindHostOf(int lobbyId) {
            return LobbyList.Where(x => x.LobbyId == lobbyId).FirstOrDefault()?.Host;
        }

#pragma warning restore CS4014
    }
}
