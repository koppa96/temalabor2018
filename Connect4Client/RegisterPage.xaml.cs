using Newtonsoft.Json.Linq;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http.Filters;
using Windows.Web.Http;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Connect4Client {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page {
        public RegisterPage() {
            this.InitializeComponent();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e) {
            JObject jObject = new JObject {
                { "Username", tbUsername.Text },
                { "Email", tbEmail.Text },
                { "Password", pwbPassword.Password },
                { "ConfirmPassword", pwbConfirmPassword.Password }
            };

            string json = jObject.ToString();
            string url = "https://localhost:44301/Account/Register";
            HttpStringContent content = new HttpStringContent(json, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");

            HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
            filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Expired);
            filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Untrusted);
            filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.InvalidName);

            using (var client = new HttpClient(filter)) {
                Uri uri = new Uri(url);
                HttpResponseMessage responseMessage = await client.PostAsync(uri, content);

                if (responseMessage.StatusCode == HttpStatusCode.Ok) {
                    string token = await responseMessage.Content.ReadAsStringAsync();

                    MessageDialog dialog = new MessageDialog(token) {
                        Title = "Token received from server"
                    };

                    await dialog.ShowAsync();
                }
            }
        }
    }
}
