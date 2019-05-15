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
using Czeum.ClientCallback;
using Czeum.Abstractions.DTO;

namespace Czeum.Client.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private IUserManagerService userManagerService;
        private INavigationService navigationService;
        private IDialogService dialogService;
        private string name;
        public string Name {
            get => name;
            set => SetProperty(ref name, value);
        }
        private string password;
        public string Password { 
			get => password;
            set => SetProperty(ref password, value);
        }
        private string confirmPassword;
        public string ConfirmPassword {
			get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }
        private string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
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
            bool result = await userManagerService.RegisterAsync(new DTO.UserManagement.RegisterModel { Username = Name, Password = Password, Email = Email, ConfirmPassword = ConfirmPassword });
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
