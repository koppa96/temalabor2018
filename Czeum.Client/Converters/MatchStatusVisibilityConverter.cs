using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Czeum.Client.Converters {
    class MatchStatusVisibilityConverter : IValueConverter{
        public object Convert(object value, Type targetType, object parameter, string language) {
            /*var status = value as Connect4Dtos.GameState?;
            if(status == null || status.Value != Connect4Dtos.GameState.YourTurn) {
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
