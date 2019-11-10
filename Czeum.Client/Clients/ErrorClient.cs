using Czeum.Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Czeum.Client.Clients
{/*
    class ErrorClient : ClientCallback.IErrorClient
    {
        private IDialogService dialogService;

        public async Task ReceiveError(string errorCode)
        {
            dialogService.ShowError(errorCode);
        }

        public ErrorClient(IDialogService dialogService, IHubService hubService)
        {
            this.dialogService = dialogService;
            hubService.Connection.On<string>(nameof(ReceiveError), ReceiveError);
        }
    }*/
}
