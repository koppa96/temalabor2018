using Connect4Client.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient()) {
                HttpResponseMessage responseMessage = await client.PostAsync(url, content);

                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK) {
                    string token = await responseMessage.Content.ReadAsStringAsync();

                    MessageDialog dialog = new MessageDialog(token);
                    dialog.Title = "Token received from server";

                    await dialog.ShowAsync();
                }
            }
        }
    }
}
