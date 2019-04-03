using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
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
using Newtonsoft.Json.Linq;
using MessageDialog = Windows.UI.Popups.MessageDialog;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Czeum.Client {/*
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page {
	    private ContentDialog loadingDialog;
        public SettingsPage() {
            this.InitializeComponent();

	        loadingDialog = new ContentDialog() {
		        Content = new ProgressRing() {
			        IsActive = true,
			        HorizontalAlignment = HorizontalAlignment.Stretch,
			        VerticalAlignment = VerticalAlignment.Stretch
		        },
	        };
		}

	    private async void ChangePasswordButton_OnClick(object sender, RoutedEventArgs e) {
		    loadingDialog.ShowAsync();
		    JObject jObject = new JObject {
			    {"OldPassword", OldPasswordBox.Password},
			    {"Password", NewPasswordBox.Password},
			    {"ConfirmPassword", ConfirmNewPasswordBox.Password}
		    };

		    OldPasswordBox.Password = "";
		    NewPasswordBox.Password = "";
		    ConfirmNewPasswordBox.Password = "";

			string json = jObject.ToString();
		    string url = App.AppUrl + "/Account/ChangePassword";

		    HttpStringContent content = new HttpStringContent(json, UnicodeEncoding.Utf8, "application/json");

		    HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
		    filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Expired);
		    filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Untrusted);
		    filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.InvalidName);

		    using (HttpClient client = new HttpClient(filter)) {
			    Uri uri = new Uri(url);
			    client.DefaultRequestHeaders.TryAppendWithoutValidation("Authorization", "bearer " + App.Token);
			    HttpResponseMessage response = await client.PostAsync(uri, content);

				var resourceLoader = ResourceLoader.GetForViewIndependentUse();
				if (response.IsSuccessStatusCode) {
					MessageDialog successDialog = new MessageDialog(resourceLoader.GetString("SuccessMessage")) {
						Title = resourceLoader.GetString("Success")
					};

					loadingDialog.Hide();
				    await successDialog.ShowAsync();
			    } else {
				    MessageDialog failDialog = new MessageDialog(resourceLoader.GetString("FailMessage")) {
					    Title = resourceLoader.GetString("Fail")
				    };

					loadingDialog.Hide();
				    await failDialog.ShowAsync();
			    }
		    }
		}
    }*/
}
