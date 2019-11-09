using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace Czeum.Client.Converters
{
    class FriendStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var status = value as bool?;
            if(status == null && !status.HasValue)
            {
                return null;
            }
            if (status.Value)
            {
                return Colors.Green;
            }
            return Colors.DarkGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
