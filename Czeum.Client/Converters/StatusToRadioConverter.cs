using Connect4Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Czeum.Client.Converters {
    class StatusToRadioConverter : IValueConverter{
        public object Convert(object value, Type targetType, object parameter, string language) {
            var status = value as LobbyStatus?;
            if((status == null) || (status.Value == LobbyStatus.Public)) {
                return false;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
