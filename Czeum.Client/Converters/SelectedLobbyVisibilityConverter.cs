using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Czeum.Client.Converters
{
    class SelectedLobbyVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) {
            var selectedItem = value as int?;
            if((selectedItem == null) || (selectedItem.Value == -1) ){
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
