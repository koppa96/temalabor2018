using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Czeum.Client.Interfaces;

namespace Czeum.Client.Services {
    class DialogService : IDialogService
    {
        public IAsyncOperation<ContentDialogResult> ShowConfirmation(string message)
        {
            var contentDialog = new ContentDialog()
            {
                Title="Please confirm your action", 
                Content = message,
                PrimaryButtonText = "Ok",
                CloseButtonText = "Cancel"
                
            };
            return contentDialog.ShowAsync();
        }
    }
}
