using Connect4Client.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Connect4Client {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page {

        private ObservableCollection<LobbyData> _lobbies;
        private ObservableCollection<LobbyData> Lobbies { get { return _lobbies; } }
        private LobbyData selectedLobby;

        public HomePage() {
            this.InitializeComponent();
            _lobbies = new ObservableCollection<LobbyData>();
            _lobbies.Add(new LobbyData { Id = 213, Leader = "Maca", Open = true , InvitedPlayers = { "Laca", "Papa" } });
            _lobbies.Add(new LobbyData { Id = 45, Leader = "RandomUser#123", Open = true });
            _lobbies.Add(new LobbyData { Id = 2052, Leader = "PrivacyFan", Open = false, InvitedPlayers = {"Maca2" }});
            _lobbies.Add(new LobbyData { Id = 21893, Leader = "Maca2", Open = true });

        }
        
    }
}
