using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http.Filters;
using Windows.Web.Http;
using Windows.ApplicationModel.Resources;
using Windows.UI.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Connect4Client {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page {
        private ContentDialog loadingDialog;
        public RegisterPage() {
            this.InitializeComponent();

            loadingDialog = new ContentDialog() {
                Content = new ProgressRing() {
                    IsActive = true,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                },
            };

            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            currentView.BackRequested += BackButton_Click;
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e) {
            loadingDialog.ShowAsync();
            JObject jObject = new JObject {
                { "Username", tbUsername.Text },
                { "Email", tbEmail.Text },
                { "Password", pwbPassword.Password },
                { "ConfirmPassword", pwbConfirmPassword.Password }
            };

            string json = jObject.ToString();
            string url = App.AppUrl + "/Account/Register";
            HttpStringContent content = new HttpStringContent(json, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");

            HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
            filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Expired);
            filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Untrusted);
            filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.InvalidName);

            using (var client = new HttpClient(filter)) {
                Uri uri = new Uri(url);
                HttpResponseMessage responseMessage = await client.PostAsync(uri, content);

                if (responseMessage.StatusCode == HttpStatusCode.Ok) {
                    App.Token = await responseMessage.Content.ReadAsStringAsync();

                    await OnSuccessfulRegister();
                    loadingDialog.Hide();
                } else {
                    var resourceLoader = ResourceLoader.GetForViewIndependentUse();
					string response = await responseMessage.Content.ReadAsStringAsync();

                    pwbPassword.Password = pwbConfirmPassword.Password = "";
					MessageDialog dialog = new MessageDialog(resourceLoader.GetString(response)) {
						Title = resourceLoader.GetString("RegisterError")
                    };

                    loadingDialog.Hide();
                    await dialog.ShowAsync();
                }
            }
        }

        private async Task OnSuccessfulRegister() {
            await ConnectionManager.Instance.CreateConnection();
            var lobbyList = await ConnectionManager.Instance.GetLobbies();
            var matchList = await ConnectionManager.Instance.GetMatches();
            LobbyRepository.Instance.LoadItems(lobbyList);
            MatchRepository.Instance.LoadItems(matchList);
            ConnectionManager.Instance.UserName = tbUsername.Text;
            Frame.Navigate(typeof(MainPage));
        }

        private void BackButton_Click(object sender, BackRequestedEventArgs e) {
            Frame.Navigate(typeof(LoginPage));

            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Disabled;

            currentView.BackRequested -= BackButton_Click;
        }
    }
}
