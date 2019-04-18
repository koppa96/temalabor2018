using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Czeum.Client.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Prism.Windows.Navigation;

namespace Czeum.Client.Services {
    class HubService : IHubService {
        private INavigationService navigationService;
        private IDialogService dialogService;

        private string BASE_URL = "https://localhost:44301";
        private IUserManagerService userManagerService;

        public HubConnection Connection { get; private set; }

        public HubService(INavigationService navigationService, IDialogService dialogService, IUserManagerService userManagerService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.userManagerService = userManagerService;
        }

        public void CreateHubConnection()
        {
            Connection = new HubConnectionBuilder()
                .WithUrl(Flurl.Url.Combine(BASE_URL, "/gamehub"), options => {
                    options.AccessTokenProvider = () =>
                        Task.FromResult(userManagerService.AccessToken);
                }).AddNewtonsoftJsonProtocol(protocol => {
                    protocol.PayloadSerializerSettings.TypeNameHandling = TypeNameHandling.All;
                })
                .Build();
        }

        public async Task ConnectToHubAsync()
        {
            await Connection.StartAsync();
        }

        public async Task DisconnectFromHub()
        {
            await Connection.StopAsync();
        }
    
    }
}
