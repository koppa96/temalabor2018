using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Czeum.Client.Converters {
    class MatchStatusSymbolConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            /* var status = value as Czeum.DTO.GameState?;
                if(status != null) {
                    if(status.Value == Czeum.DTO.GameState.YouWon) {
                        return Symbol.Like;
                    }
                    else if(status.Value == Czeum.DTO.GameState.EnemyWon) {
                        return Symbol.Dislike;
                    }
                    else if(status.Value == Czeum.DTO.GameState.Draw) {
                        return Symbol.LikeDislike;
                    }
                }*/
            return Symbol.Target;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
