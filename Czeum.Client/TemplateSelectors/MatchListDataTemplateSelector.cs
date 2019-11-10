using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Chess;
using Czeum.Core.DTOs.Connect4;
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
                return null;
            }

            var match = (MatchStatus)item;

            if (match.CurrentBoard.Content is ChessMoveResult)
            {
                return ChessDataTemplate;
            }
            else if (match.CurrentBoard.Content is Connect4MoveResult)
            {
                return Connect4DataTemplate;
            }
            return null;
        }
    }
}
