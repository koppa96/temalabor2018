using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Czeum.Client.Interfaces
{
    interface ILoginPageViewModel
    {
        String Name { get; set; }
        String Password { get; set; }
        String ConfirmPassword { get; set; }
        String Email { get; set; }
        Visibility PasswordRepeatVisibility { get; set; }

        ICommand ToggleClickCommand
        {
            get;
        }

        ICommand PerformClickCommand
        {
            get;
        }
    }
}
