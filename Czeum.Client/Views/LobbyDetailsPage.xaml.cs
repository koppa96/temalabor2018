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
using Czeum.Client.Interfaces;
using Czeum.Client.ViewModels;
using Prism.Windows.Mvvm;
using System.ComponentModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Czeum.Client.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LobbyDetailsPage : SessionStateAwarePage, INotifyPropertyChanged{
        public LobbyDetailsPage() {
            this.InitializeComponent(); DataContextChanged += LobbyDetailsPageControl_DataContextChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public LobbyDetailsPageViewModel ConcreteDataContext => DataContext as LobbyDetailsPageViewModel;

        private void LobbyDetailsPageControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConcreteDataContext)));
        }

        private void MessageText_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                // Hmmm, such lovely UWP stuff
                ConcreteDataContext.MessageText = (sender as TextBox).Text;
                ConcreteDataContext.SendMessageCommand.Execute(null);
            }
        }
    }
}
