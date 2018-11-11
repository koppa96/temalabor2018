using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using System.Diagnostics;
using Windows.UI.Xaml;

namespace Connect4Client.Converters {
    public class LobbyStatusConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            Boolean? v = value as Boolean?;
            if(v == true) {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
