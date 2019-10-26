using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.Client.Interfaces;
using Czeum.Core.DTOs.Abstractions.Lobbies;
using Czeum.Core.DTOs.Wrappers;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Czeum.Client.Models {

    /// <summary>
    /// Stores a local cache of all the lobbies.
    /// Also keeps track of the currently joined lobby.
    /// </summary>
    class LobbyStore : ILobbyStore, INotifyPropertyChanged
    {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        public ObservableCollection<LobbyData> LobbyList { get; private set; }
        private LobbyData selectedLobby;
        public LobbyData SelectedLobby {
            get => selectedLobby;
            set {
                selectedLobby = value;

                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { 
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedLobby")); 
                });
            }
        }

        public LobbyStore(){
            LobbyList = new ObservableCollection<LobbyData>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task AddLobby(LobbyData lobby)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { 
                LobbyList.Add(lobby); 
            });
        }

        public async Task AddLobbies(IEnumerable<LobbyData> lobbies)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                foreach(var lobby in lobbies)
                {
                    // In lieu of InsertRange/AddRange/whatever
                    LobbyList.Add(lobby);
                }
            });
        }

        public async Task RemoveLobby(Guid lobbyId)
        {
            var lobbyToRemove = LobbyList.FirstOrDefault(x => x.Id == lobbyId);
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                LobbyList.Remove(lobbyToRemove);
                if(selectedLobby?.Id == lobbyId)
                {
                    SelectedLobby = null;
                }
            }); 
        }

        public async Task UpdateLobby(LobbyData lobby)
        {
            var lobbyToUpdate = LobbyList.FirstOrDefault(x => x.Id == lobby.Id);
            int index = LobbyList.IndexOf(lobbyToUpdate);
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                //dirty hack to refresh item in the list
                LobbyList.RemoveAt(index);
                LobbyList.Insert(index, lobby);
                
                if((selectedLobby != null) && (selectedLobby?.Id == lobby.Id))
                {
                    SelectedLobby = lobby;
                }
            });
        }

        public async Task ClearLobbies()
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { 
                LobbyList.Clear(); 
            });
        }
    }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

}
