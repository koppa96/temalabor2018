using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.Client.Interfaces;
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
        public ObservableCollection<LobbyData> LobbyList { get; private set; }
        private LobbyData selectedLobby;
        public LobbyData SelectedLobby {
            get => selectedLobby;
            set {
                selectedLobby = value;
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedLobby")); });

            }
        }

        public LobbyStore(){
            LobbyList = new ObservableCollection<LobbyData>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task AddLobby(LobbyData lobby)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { LobbyList.Add(lobby); });
        }

        public async Task RemoveLobby(int lobbyId)
        {
            var lobbyToRemove = LobbyList.FirstOrDefault(x => x.LobbyId == lobbyId);
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { LobbyList.Remove(lobbyToRemove); }); 
        }

        public async Task UpdateLobby(LobbyData lobby)
        {
            var lobbyToUpdate = LobbyList.FirstOrDefault(x => x.LobbyId == lobby.LobbyId);
            int index = LobbyList.IndexOf(lobbyToUpdate);
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                //dirty hack to refresh item in the list
                LobbyList.RemoveAt(index);
                LobbyList.Insert(index, lobby);

                if((selectedLobby != null) && (selectedLobby.LobbyId == lobby.LobbyId))
                {
                    SelectedLobby = lobby;
                }
            });
        }

        public async Task ClearLobbies()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { LobbyList.Clear(); });
        }
    }
}
