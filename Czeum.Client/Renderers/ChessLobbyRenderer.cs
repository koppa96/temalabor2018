using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Czeum.Abstractions.DTO;
using Czeum.Client.Interfaces;
using Czeum.DTO.Chess;

namespace Czeum.Client {
    [LobbyRenderer(typeof(ChessLobbyData))]
    class ChessLobbyRenderer : ILobbyRenderer {
        public Panel RenderLobby(LobbyData lobbyData)
        {
            StackPanel panel = new StackPanel();

            panel.Children.Add(new TextBlock(){Text = "CHESS"});

            return panel;
        }
    }
}
