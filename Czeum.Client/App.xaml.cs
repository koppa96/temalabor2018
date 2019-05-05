using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Prism.Unity.Windows;
using System.Threading.Tasks;
using Czeum.Client.Interfaces;
using Czeum.Client.Services;
using Microsoft.Practices.Unity;
using Prism.Logging;
using NLog;
using Prism.Windows.Navigation;
using Czeum.Client.Models;
using Czeum.ClientCallback;
using Czeum.Client.Clients;

namespace Czeum.Client
{
    sealed partial class App : PrismUnityApplication
    {
   
        public static string Token { get; set; }
        public static readonly string AppUrl = "https://koppa96.sch.bme.hu/Czeum.Server";

        public App()
        {
            this.InitializeComponent();
        }


        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            Container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IUserManagerService, UserManagerService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ILoggerFacade, NLogAdapter>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ILobbyService, LobbyService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ILobbyStore, LobbyStore>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ILobbyClient, LobbyClient>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IHubService, HubService>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IGameClient, GameClient>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IMatchService, MatchService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IMatchStore, MatchStore>(new ContainerControlledLifetimeManager());
            return Task.FromResult<object>(null);
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            
            this.NavigationService.Navigate(PageTokens.Login.ToString(), null);
            return Task.FromResult<object>(null);
        }

        protected override UIElement CreateShell(Frame rootFrame)
        {
            var shell = Container.Resolve<AppShell>();
            shell.SetContentFrame(rootFrame);
            return shell;
        }
        protected override object Resolve(Type type)
        {
            return base.Resolve(type);
        }
    }

    class NLogAdapter : ILoggerFacade
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();
        public NLogAdapter()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logdebug = new NLog.Targets.DebuggerTarget("logdebug");
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logdebug);
            LogManager.Configuration = config;
        }

        public void Log(string message, Category category, Priority priority)
        {
            switch (category)
            {
                case Category.Debug:
                    _logger.Debug(message);
                    break;
                case Category.Exception:
                    _logger.Error(message);
                    break;
                case Category.Info:
                    _logger.Info(message);
                    break;
                case Category.Warn:
                    _logger.Warn(message);
                    break;
            }
        }
    }

}
