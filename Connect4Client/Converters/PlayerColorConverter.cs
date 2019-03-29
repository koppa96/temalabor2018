using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Czeum.Client.Converters {
    class PlayerColorConverter : IValueConverter{
        public object Convert(object value, Type targetType, object parameter, string language) {
            var playerItem = value as Connect4Dtos.Item?;
            if(playerItem == null) {
                return new SolidColorBrush(Windows.UI.Colors.White);
            }
            if(playerItem.Value == Connect4Dtos.Item.Red) {
                return new SolidColorBrush(Windows.UI.Colors.Red);
            }
            return new SolidColorBrush(Windows.UI.Colors.Yellow);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
