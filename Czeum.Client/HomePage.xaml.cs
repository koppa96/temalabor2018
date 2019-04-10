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

namespace Czeum.Client {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /*public sealed partial class HomePage : Page {
        public HomePage() {
            this.InitializeComponent();
        }

        private void LobbyButton_Click(object sender, RoutedEventArgs e) {
            this.Frame.Navigate(typeof(LobbyBrowserPage));
        }

        private void SoloButton_Click(object sender, RoutedEventArgs e) {
            if (SoloBtn.IsChecked.Value) {
                ConnectionManager.Instance.JoinSoloQueue();
                SoloTxt.Text = "You are now queued. You'll leave the queue on leaving this page.";
            }
            else {
                ConnectionManager.Instance.LeaveSoloQueue();
                SoloTxt.Text = "Click here to queue into a match";
            }
        }

        private void MatchButton_Click(object sender, RoutedEventArgs e) {
            this.Frame.Navigate(typeof(MatchesPage));
        }

        private void StatsButton_Click(object sender, RoutedEventArgs e) {
            this.Frame.Navigate(typeof(StatisticsPage));
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e) {
            if (SoloBtn.IsChecked.Value) {
                ConnectionManager.Instance.LeaveSoloQueue();
            }
            base.OnNavigatingFrom(e);
        }
    }*/
}
