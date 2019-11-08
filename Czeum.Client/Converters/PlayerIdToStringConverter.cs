using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Chess;
using Czeum.Core.DTOs.Connect4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Czeum.Client.Converters
{
    class PlayerIdToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var match = value as MatchStatus;
            if(match == null)
            {
                return "";
            }
            if(match.CurrentBoard.Content is ChessMoveResult)
            {
                return match.PlayerIndex == 0 ? "white" : "black";
            }   
            else if(match.CurrentBoard.Content is Connect4MoveResult)
            {
                return match.PlayerIndex == 0 ? "red" : "yellow";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
