﻿using Connect4Client.DTOs;
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

using Connect4Dtos;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Connect4Client {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LobbyBrowserPage : Page {
        
        private ObservableCollection<LobbyData> Lobbies { get { return LobbyRepository.Instance.LobbyList; } }
        private LobbyData SelectedLobby {
            get { return LobbyRepository.Instance.SelectedLobby; }
            set { LobbyRepository.Instance.SelectedLobby = value; }
        }

        public LobbyBrowserPage() {
            this.InitializeComponent();
            LobbyRepository.Instance.AddHomePage(this);
        }

        private void JoinButton_Click(object sender, RoutedEventArgs e) {
            ConnectionManager.Instance.ConnectToLobby(LobbyRepository.Instance.SelectedLobby.LobbyId);
        }

        public void SuccessfulLobbyJoin() {
            this.Frame.Navigate(typeof(LobbyPage));
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e) {
            ConnectionManager.Instance.CreateLobby(LobbyStatus.Public);
        }
    }
}