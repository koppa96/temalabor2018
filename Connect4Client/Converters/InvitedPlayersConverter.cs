using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Czeum.Client.Converters {
    class InvitedPlayersConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            var invitedPlayers = value as List<String>;
            if(value == null) {
                return "No players invited";
            }
            return string.Join("\n", invitedPlayers);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
