using Czeum.Abstractions.DTO;
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
                case GameState.Draw: return "Match ended in a draw";
                case GameState.EnemyTurn: return "It's your opponent's turn";
                case GameState.EnemyWon: return "Your opponent won";
                case GameState.YourTurn: return "It's your turn";
                case GameState.YouWon: return "You won";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
