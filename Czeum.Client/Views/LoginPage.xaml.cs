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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : SessionStateAwarePage {
        //private ContentDialog loadingDialog;
        public LoginPage()
        {
            this.InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            //Frame.Navigate(typeof(RegisterPage));
        }

        /*
        loadingDialog = new ContentDialog() {
            Content = new ProgressRing() {
                IsActive = true,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            },
        };

        ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
        titleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;

        CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
        coreTitleBar.ExtendViewIntoTitleBar = true;
    }

    private void RegisterButton_Click(object sender, RoutedEventArgs e) {
        Frame.Navigate(typeof(RegisterPage));
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e) {
        LoginAsyc();
    }

    private async void LoginAsyc() {
        loadingDialog.ShowAsync();

        JObject jObject = new JObject();
        jObject.Add("Username", tbUsername.Text);
        jObject.Add("Password", pwbPassword.Password);

        string json = jObject.ToString();
        string url = App.AppUrl + "/Account/Login";
        HttpStringContent content = new HttpStringContent(json, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");

        HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
        filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Expired);
        filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Untrusted);
        filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.InvalidName);

        using (var client = new HttpClient(filter)) {
            Uri uri = new Uri(url);
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try {
                responseMessage = await client.PostAsync(uri, content);
            } catch (System.Runtime.InteropServices.COMException e) {
                loadingDialog.Hide();
                ContentDialog errorDialog = new ContentDialog() {
                    Title = "Could not connect to the server!",
                    Content = "The server can not be reached. Please check your internet connection or contact the developers.",
                    CloseButtonText = "Ok",
                };
                errorDialog.ShowAsync();
                return;
            }


            if (responseMessage.StatusCode == HttpStatusCode.Ok) {
                App.Token = await responseMessage.Content.ReadAsStringAsync();

                await OnSuccessfulLogin();
                loadingDialog.Hide();

            } else {
                var resourceLoader = ResourceLoader.GetForViewIndependentUse();
                pwbPassword.Password = "";

                MessageDialog dialog = new MessageDialog(resourceLoader.GetString("LoginError")) {
                    Title = resourceLoader.GetString("Error")
                };

                loadingDialog.Hide();
                await dialog.ShowAsync();
            }
        }
    }

    private async Task OnSuccessfulLogin() {
        await ConnectionManager.Instance.CreateConnection();
        var lobbyList = await ConnectionManager.Instance.GetLobbies();
        var matchList = await ConnectionManager.Instance.GetMatches();
        LobbyRepository.Instance.LoadItems(lobbyList);
        MatchRepository.Instance.LoadItems(matchList);
        ConnectionManager.Instance.UserName = tbUsername.Text;
        Frame.Navigate(typeof(MainPage));
    }

    private void pwbPassword_KeyDown(object sender, KeyRoutedEventArgs e) {
        if (e.Key == VirtualKey.Enter) {
            LoginAsyc();
        }
    }*/
    }
}
