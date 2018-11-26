using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Connect4Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ResourceLoader resourceLoader;
        Dictionary<string, Type> pages;

        public MainPage()
        {
	        resourceLoader = ResourceLoader.GetForViewIndependentUse();
			this.InitializeComponent();

	        pages = new Dictionary<string, Type>();
	        
			pages.Add(resourceLoader.GetString("HomeItemContent"), typeof(HomePage));
			pages.Add(resourceLoader.GetString("LobbyItemContent"), typeof(LobbyBrowserPage));
			pages.Add(resourceLoader.GetString("MatchesItemContent"), typeof(MatchesPage));
			pages.Add(resourceLoader.GetString("StatisticsItemContent"), typeof(StatisticsPage));
        }

        private void NavigationViewControl_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args) {
            MatchRepository.Instance.SelectedMatch = null;
            if (args.IsSettingsInvoked) {
                ContentFrame.Navigate(typeof(SettingsPage));
                return;
            }

            string menuName = (string)args.InvokedItem;
            ContentFrame.Navigate(pages[menuName]);
        }

        private void NavigationViewControl_Loaded(object sender, RoutedEventArgs e) {
            ContentFrame.Navigate(typeof(HomePage));
        }

        private async void Logout_Tapped(object sender, TappedRoutedEventArgs e) {
            MessageDialog logoutDialog = new MessageDialog(resourceLoader.GetString("LogoutDialogMessage"));

            logoutDialog.Commands.Add(new UICommand(resourceLoader.GetString("Yes"), new UICommandInvokedHandler(CommandInvokedHandler), 1));
            logoutDialog.Commands.Add(new UICommand(resourceLoader.GetString("No"), new UICommandInvokedHandler(CommandInvokedHandler), 2));

            await logoutDialog.ShowAsync();
        }
        
        private void CommandInvokedHandler(IUICommand command) {
            if ((int)command.Id == 1) {
                LogoutAsync();
            }
        }

        private async void LogoutAsync() {
            string url = App.AppUrl + "/Account/Logout";

            HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
            filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Expired);
            filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Untrusted);
            filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.InvalidName);

            HttpStringContent content = new HttpStringContent("");

            using (HttpClient client = new HttpClient(filter)) {
                Uri uri = new Uri(url);
                client.DefaultRequestHeaders.TryAppendWithoutValidation(
                    "Authorization",
                    "bearer " + App.Token);
                HttpResponseMessage message = await client.PostAsync(uri, content);

                if (message.IsSuccessStatusCode) {
                    Frame.Navigate(typeof(LoginPage));
                    await ConnectionManager.Instance.CloseConnectionAsync();
                } else {
                    ContentDialog dialog = new ContentDialog() {
                        Content = resourceLoader.GetString("LogoutFailed"),
                        CloseButtonText = resourceLoader.GetString("Ok")
                    };

                    await dialog.ShowAsync();
                }
            }
        }

    }
}
