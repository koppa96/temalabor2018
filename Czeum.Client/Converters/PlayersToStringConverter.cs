using Czeum.Core.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Czeum.Client.Converters
{
    class PlayersToStringConverter : IValueConverter, INotifyPropertyChanged
    {
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

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var players = (value as List<Player>)?.Select(p => p.Username)?.Where(p => p != Username);
            if(players == null) { 
                return ""; 
            }
            return string.Join(", ", players);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
