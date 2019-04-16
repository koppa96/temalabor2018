using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Czeum.DTO.Chess;
using Czeum.DTO.Connect4;

namespace Czeum.Client.TemplateSelectors {
    class LobbyDetailsDataTemplateSelector : DataTemplateSelector{
        public DataTemplate ChessDataTemplate { get; set; }
        public DataTemplate Connect4DataTemplate { get; set; }
        
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item == null)
            {
                return null; 
            }

            if (item is ChessLobbyData) {
                return ChessDataTemplate;
            }
            else if (item is Connect4LobbyData) {
                return Connect4DataTemplate;
            }
            return null;
        }
    }
}
