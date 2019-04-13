using Czeum.Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Windows.Mvvm;
using System.ComponentModel;
using System.Windows.Input;
using Prism.Commands;
using Microsoft.Practices.Unity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Prism.Windows.Navigation;

namespace Czeum.Client.ViewModels
{
    class LoginPageViewModel : ViewModelBase
    {
        private IUserManagerService userManagerService;
        private INavigationService navigationService;
        private IDialogService dialogService;

        private string _Name;
        public string Name {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }
        private string _Password;
        public string Password { 
			get => _Password;
            set => SetProperty(ref _Password, value);
        }
        private string _ConfirmPassword;
        public string ConfirmPassword {
			get => _ConfirmPassword;
            set => SetProperty(ref _ConfirmPassword, value);
        }
        private string _Email;
        public string Email
        {
            get => _Email;
            set => SetProperty(ref _Email, value);
        }

        public ICommand LoginCommand { get; private set; }
        public ICommand RegisterCommand { get; private set; }

        private async void LoginAsync()
        {
            dialogService.ShowLoadingDialog();
            bool result = await userManagerService.LoginAsync(new DTO.UserManagement.LoginModel { Username = Name, Password = Password });
            if (result) {
                navigationService.Navigate("Lobby", null);
            }
            else {
                await dialogService.ShowError("Login failed. Please try again.");
            }
            dialogService.HideLoadingDialog();
            
        }

        private async void RegisterAsync() {
            dialogService.ShowLoadingDialog();
            bool result = await userManagerService.RegisterAsync(new DTO.UserManagement.RegisterModel { Username = Name, Password = Password, ConfirmPassword = ConfirmPassword });
            if (result) {
                await dialogService.ShowSuccess("Registration completed successfully. You can now log in with your new account.");
            }
            else {
                await dialogService.ShowError("Registration failed. Please try again.");
            }
            dialogService.HideLoadingDialog();
        }

        public LoginPageViewModel(IUserManagerService userManagerService, INavigationService navigationService, IDialogService dialogService)
        {
            this.navigationService = navigationService;   
            this.userManagerService = userManagerService;
            this.dialogService = dialogService;

            LoginCommand = new DelegateCommand(LoginAsync);
            RegisterCommand = new DelegateCommand(RegisterAsync);
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            navigationService.ClearHistory(); //This way the back button disappears after logging out
            base.OnNavigatedTo(e, viewModelState);
        }
    }
}
