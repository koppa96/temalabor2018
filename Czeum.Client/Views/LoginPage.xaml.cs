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


namespace Czeum.Client.Views {
    public sealed partial class LoginPage : SessionStateAwarePage {
        public LoginPage()
        {
            this.InitializeComponent();
        }
    }
}
