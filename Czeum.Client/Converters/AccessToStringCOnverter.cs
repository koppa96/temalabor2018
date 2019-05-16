using Czeum.Abstractions.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Czeum.Client.Converters
{
    class AccessToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            LobbyAccess? access = value as LobbyAccess?;
            switch (access)
            {
                case LobbyAccess.Public: return "\ue785";
                case LobbyAccess.FriendsOnly: return "\ue779";
                case LobbyAccess.Private: return "\ue72e";
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
