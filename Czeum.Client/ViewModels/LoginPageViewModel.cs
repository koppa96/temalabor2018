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
using Prism.Windows.Navigation;

namespace Czeum.Client.ViewModels
{
    class LoginPageViewModel : ViewModelBase, ILoginPageViewModel, INotifyPropertyChanged
    {
        private enum PageState { Login, Register}
        private PageState pageState = PageState.Login;
        private IUserManagerService userManagerService;
        private INavigationService navigationService;

        private string _Name;
        public string Name {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }
        private string m_Password;
        public string Password { 
			get => m_Password;
            set => SetProperty(ref m_Password, value);
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
        private Visibility _RegistrationInfoVisibility = Visibility.Collapsed;
        public Visibility RegistrationInfoVisibility
        {
            get => _RegistrationInfoVisibility;
            set => SetProperty(ref _RegistrationInfoVisibility, value);
        }
        

        public ICommand ToggleClickCommand { get; private set; }
        public ICommand PerformClickCommand { get; private set; }
        

        private async void PerformClickAsync()
        {
            if (pageState == PageState.Login)
            {
                bool result = await userManagerService.LoginAsync(new DTO.UserManagement.LoginModel { Username = Name, Password = Password });
                if(result) {
                    navigationService.Navigate("Lobby", null); 
                }
                else {
                    throw new NotImplementedException();
                }
                
            }
            else
            {
                await userManagerService.RegisterAsync(new DTO.UserManagement.RegisterModel { Username = Name, Password = Password, ConfirmPassword = ConfirmPassword });
            }
        }

        private void ToggleClick()
        {
            switch (pageState)
            {
                case PageState.Register:
                    RegistrationInfoVisibility = Visibility.Collapsed;
                    pageState = PageState.Login;
                    break;
                case PageState.Login:
                    RegistrationInfoVisibility = Visibility.Visible;
                    pageState = PageState.Register;
                    break;
            }
        }

        public LoginPageViewModel(IUserManagerService userManagerService, INavigationService navigationService)
        {
            this.navigationService = navigationService;   
            this.userManagerService = userManagerService;
            PerformClickCommand = new DelegateCommand(PerformClickAsync);
            ToggleClickCommand = new DelegateCommand(ToggleClick);
            RegistrationInfoVisibility = Visibility.Collapsed;
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            navigationService.ClearHistory(); //This way the back button disappears after logging out
            base.OnNavigatedTo(e, viewModelState);
        }
    }
}
