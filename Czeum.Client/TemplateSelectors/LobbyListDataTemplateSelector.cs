using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Czeum.Abstractions.DTO;
using Czeum.DTO.Chess;
using Czeum.DTO.Connect4;

namespace Czeum.Client.TemplateSelectors {
    class LobbyListDataTemplateSelector : DataTemplateSelector{
        public DataTemplate ChessDataTemplate { get; set;}
        public DataTemplate Connect4DataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject dependencyObject)
        {
            if (item == null)
            {
                throw new ArgumentException("Tried selecting template for null lobby");
            }

            if (item is ChessLobbyData)
            {
                return ChessDataTemplate;
            }
            else if (item is Connect4LobbyData)
            {
                return Connect4DataTemplate;
            }
            throw new ArgumentException("No template found for this type.");
        }
    }
}
