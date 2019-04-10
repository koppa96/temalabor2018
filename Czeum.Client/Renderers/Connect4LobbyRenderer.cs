using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Czeum.Abstractions.DTO;
using Czeum.Client.Interfaces;
using Czeum.DTO.Connect4;

namespace Czeum.Client {
    [LobbyRenderer(typeof(Connect4LobbyData))]
    class Connect4LobbyRenderer : ILobbyRenderer{
        public Panel RenderLobby(LobbyData lobbyData)
        {
            var typedLobbyData = (Connect4LobbyData) lobbyData;
            StackPanel panel = new StackPanel();

            TextBox widthTextBox = new TextBox()
            {
                PlaceholderText = "Board width"
            };
            Binding widthBinding = new Binding()
            {
                Source = typedLobbyData,
                Path = new PropertyPath(nameof(typedLobbyData.BoardWidth)),
                Mode = BindingMode.TwoWay
            };
            widthTextBox.SetBinding(TextBox.TextProperty, widthBinding);
            
            TextBox heightTextBox = new TextBox()
            {
                PlaceholderText = "Board height"
            };
            Binding heightBinding = new Binding()
            {
                Source = typedLobbyData,
                Path = new PropertyPath(nameof(typedLobbyData.BoardHeight)),
                Mode = BindingMode.TwoWay
            };
            heightTextBox.SetBinding(TextBox.TextProperty, heightBinding);

            panel.Children.Add(widthTextBox);
            panel.Children.Add(heightTextBox);


            return panel;
        }
    }
}
