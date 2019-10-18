using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using System.Diagnostics;
using Windows.UI.Xaml;

namespace Czeum.Client.Converters {
    public class LobbyStatusConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            /*LobbyStatus? v = value as LobbyStatus?;
            if(v.Value == LobbyStatus.Public) {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;*/
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
