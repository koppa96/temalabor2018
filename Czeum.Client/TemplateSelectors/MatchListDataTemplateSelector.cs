using Czeum.DTO;
using Czeum.DTO.Chess;
using Czeum.DTO.Connect4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Czeum.Client.TemplateSelectors
{
    class MatchListDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ChessDataTemplate { get; set; }
        public DataTemplate Connect4DataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject dependencyObject)
        {
            if (item == null)
            {
                throw new ArgumentException("Tried selecting template for null match");
            }

            var match = (MatchStatus)item;

            if (match.CurrentBoard is ChessMoveResult)
            {
                return ChessDataTemplate;
            }
            else if (match.CurrentBoard is Connect4MoveResult)
            {
                return Connect4DataTemplate;
            }
            throw new ArgumentException("No template found for this type.");
        }
    }
}
