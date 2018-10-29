using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Connect4Client {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page {
        private ContentDialog loadingDialog;
        public LoginPage() {
            this.InitializeComponent();

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

        private async void LoginButton_Click(object sender, RoutedEventArgs e) {
            loadingDialog.ShowAsync();

            JObject jObject = new JObject();
            jObject.Add("Username", tbUsername.Text);
            jObject.Add("Password", pwbPassword.Password);

            string json = jObject.ToString();
            string url = "https://localhost:44301/Account/Login";
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

                    OnSuccessfulLogin();
                    loadingDialog.Hide();
                } else {
                    var resourceLoader = ResourceLoader.GetForViewIndependentUse();

                    MessageDialog dialog = new MessageDialog(resourceLoader.GetString("LoginError")) {
                        Title = resourceLoader.GetString("Error")
                    };

                    loadingDialog.Hide();
                    await dialog.ShowAsync();
                }
            }
        }

        private void OnSuccessfulLogin() {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
