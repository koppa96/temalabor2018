using Connect4Client.DTOs;
using Connect4Dtos;
using System;
using System.Collections.Generic;
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

namespace Connect4Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LobbyPage : Page
    {

        public NotifyingLobbyData JoinedLobby { get { return LobbyRepository.Instance.JoinedLobby; } set { LobbyRepository.Instance.JoinedLobby = value; } }

        public LobbyPage()
        {
            this.InitializeComponent();
            DataContext = JoinedLobby;
            LobbyRepository.Instance.AddLobbyPage(this);
        }

        private void LeaveButton_Click(object sender, RoutedEventArgs e) {
            LeaveLobby();
        }

        private void LeaveLobby() {
            ConnectionManager.Instance.DisconnectFromLobby(JoinedLobby.LobbyId);
            LobbyRepository.Instance.LeaveLobby();
            this.Frame.GoBack();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e) {
            ConnectionManager.Instance.StartMatch(JoinedLobby.InnerLobby);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e) {
            LobbyData changedSettings = new LobbyData() {
                LobbyId = JoinedLobby.LobbyId,
                BoardHeight = Int32.Parse(rowsTb.Text),
                BoardWidth = Int32.Parse(columnsTb.Text),
                Status = privateRb.IsChecked.Value ? LobbyStatus.Private : LobbyStatus.Public
            };
            ConnectionManager.Instance.ChangeLobbySettings(changedSettings);
        }

        public void SuccessfulMatchStart() {
            this.Frame.GoBack();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            if(JoinedLobby == null) {
                return;
            }
            LeaveLobby();
        }

        private void InviteButton_Click(object sender, RoutedEventArgs e) {
            string invitedPlayer = inviteTb.Text;
            inviteTb.Text = "";
            if (invitedPlayer.Equals("")) {
                return;
            }
            if (JoinedLobby.InvitedPlayers.Contains(invitedPlayer)) {
                ContentDialog errorDialog = new ContentDialog() {
                    Title = "This player is already invited!",
                    Content = "You cannot invite a player more than one time",
                    CloseButtonText = "Ok",
                };

                errorDialog.ShowAsync();
                return;
            }
            ConnectionManager.Instance.SendInvitationTo(invitedPlayer, JoinedLobby.LobbyId);
        }

        private void CancelInviteButton_Click(object sender, RoutedEventArgs e) {
            var button = sender as Button;
            string selectedPlayer = button.DataContext as string;
            ConnectionManager.Instance.CancelInvitationOf(selectedPlayer, JoinedLobby.LobbyId);
        }

        private void KickButton_Click(object sender, RoutedEventArgs e) {
            ConnectionManager.Instance.KickGuest(JoinedLobby.LobbyId);
        }
    }
}
