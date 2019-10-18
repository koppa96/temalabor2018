using Czeum.Client.Interfaces;
using System;
using System.Collections.Generic;
using Prism.Windows.Mvvm;
using System.Windows.Input;
using Prism.Commands;
using Prism.Windows.Navigation;

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

        public string ConfirmationToken { get; set; }


        public ICommand LoginCommand { get; private set; }
        public ICommand RegisterCommand { get; private set; }
        public ICommand ConfirmCommand { get; private set; }

        private async void LoginAsync()
        {
            dialogService.ShowLoadingDialog();
            bool result = await userManagerService.LoginAsync(new Core.DTOs.UserManagement.LoginModel { Username = Name, Password = Password });
            if (result) {
                navigationService.Navigate("Lobby", null);
            }
            else {
                await dialogService.ShowError("Login failed. Please try again.");
            }
            ResetFields();
            dialogService.HideLoadingDialog();
        }

        private async void RegisterAsync() {
            dialogService.ShowLoadingDialog();
            if(ConfirmPassword != Password)
            {
                return;
            }
            bool result = await userManagerService.RegisterAsync(new Core.DTOs.UserManagement.RegisterModel { Username = Name, Password = Password, Email = Email});
            if (result) {
                await dialogService.ShowSuccess("Registration completed successfully. Please confirm your account with the token sent to your email address.");
            }
            else {
                await dialogService.ShowError("Registration failed. Please try again.");
            }
            ResetFields();
            dialogService.HideLoadingDialog();
        }


        private async void ConfirmAsync()
        {
            dialogService.ShowLoadingDialog();
            bool result = await userManagerService.ConfirmAsync(Name, ConfirmationToken);
            if (result)
            {
                await dialogService.ShowSuccess("You have successfully confirmed your registration. You can now log in to your account.");
            }
            else
            {
                await dialogService.ShowError("Process failed. Please try again.");
            }
            ResetFields();
            dialogService.HideLoadingDialog();
        }

        public LoginPageViewModel(IUserManagerService userManagerService, INavigationService navigationService, IDialogService dialogService)
        {
            this.navigationService = navigationService;   
            this.userManagerService = userManagerService;
            this.dialogService = dialogService;

            LoginCommand = new DelegateCommand(LoginAsync);
            RegisterCommand = new DelegateCommand(RegisterAsync);
            ConfirmCommand = new DelegateCommand(ConfirmAsync);
        }
        private void ResetFields()
        {
            Name = "";
            Password = "";
            ConfirmationToken = "";
            Email = "";
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            navigationService.ClearHistory(); //This way the back button disappears after logging out
            base.OnNavigatedTo(e, viewModelState);
        }
    }
}
