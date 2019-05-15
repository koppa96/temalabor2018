using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Czeum.Client.Converters {
    class InvitedPlayerNotificationConverter : IValueConverter{
        public object Convert(object value, Type targetType, object parameter, string language) {
            /*var invitedPlayers = value as List<String>;
            if (invitedPlayers.Contains(ConnectionManager.Instance.UserName)) {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;*/
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
