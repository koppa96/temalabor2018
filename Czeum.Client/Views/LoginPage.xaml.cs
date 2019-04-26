using Newtonsoft.Json.Linq;
using System;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Windows.System;
using System.Threading.Tasks;
using Prism.Windows.Mvvm;
using System.ComponentModel;
using Czeum.Client.ViewModels;

namespace Czeum.Client.Views {
    public sealed partial class LoginPage : SessionStateAwarePage, INotifyPropertyChanged {
        public LoginPage()
        {
            this.InitializeComponent();
            DataContextChanged += LoginControl_DataContextChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginPageViewModel ConcreteDataContext => DataContext as LoginPageViewModel;

        private void LoginControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConcreteDataContext)));
        }
    }
}
