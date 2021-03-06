﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Czeum.Client.Interfaces;
using ProgressRing = Windows.UI.Xaml.Controls.ProgressRing;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Czeum.Client.Services {
    public class DialogService : IDialogService
    {
        private ContentDialog progressDialog;

        public IAsyncOperation<ContentDialogResult> ShowConfirmation(string message)
        {
            var contentDialog = new ContentDialog()
            {
                Title = "Please confirm your action", 
                Content = message,
                PrimaryButtonText = "Ok",
                CloseButtonText = "Cancel"
                
            };
            return contentDialog.ShowAsync();
        }

        public IAsyncOperation<ContentDialogResult> ShowSuccess(string message) 
        {
            HideLoadingDialog();
            var contentDialog = new ContentDialog()
            {
                Title = "Success",
                Content = message,
                PrimaryButtonText = "Ok"
            };
            return contentDialog.ShowAsync();
        }

        public async Task ShowError(string message) 
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                HideLoadingDialog();
                var contentDialog = new ContentDialog()
                {
                    Title = "Error",
                    Content = message,
                    PrimaryButtonText = "Ok"
                };
                await contentDialog.ShowAsync();
            });
        }

        public void ShowLoadingDialog()
        {
            ProgressRing pr = new ProgressRing() {IsActive = true, HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch};
            progressDialog = new ContentDialog()
            {
                Content = pr
            };
            progressDialog.ShowAsync();
        }

        public void HideLoadingDialog()
        {
            progressDialog?.Hide();
            progressDialog = null;

        }

    }
}
