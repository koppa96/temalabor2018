using Czeum.Core.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Czeum.Client.TemplateSelectors
{
    class MessageTemplateSelector : DataTemplateSelector, INotifyPropertyChanged
    {
        public DataTemplate SentMessageTemplate { get; set; }
        public DataTemplate ReceivedMessageTemplate { get; set; }

        private string username = null;
        public string Username {
            get { return username; }
            set { username = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var message = item as Message;
            if (message == null)
            {
                return null;
            }

            return (message.Sender == Username) ? SentMessageTemplate : ReceivedMessageTemplate;
        }
    }
}
