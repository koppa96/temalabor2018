using Czeum.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Czeum.Client.Converters
{
    class GameStateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            GameState state = (GameState)value;
            switch (state)
            {
                case GameState.YourTurn:
                    return "It's your turn";
                case GameState.EnemyTurn:
                    return "It's your opponent's turn";
                case GameState.Won:
                    return "You won";
                case GameState.Lost:
                    return "You lost";
                case GameState.Draw:
                    return "Match ended in a draw";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}