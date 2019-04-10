using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Czeum.Abstractions.DTO;
using Czeum.Client.Interfaces;
using Czeum.DTO.Connect4;

namespace Czeum.Client {
    [LobbyRenderer(typeof(Connect4LobbyData))]
    class Connect4LobbyRenderer : ILobbyRenderer{
        public Panel RenderLobby(LobbyData lobbyData)
        {
            throw new NotImplementedException("Connect4");
        }
    }
}
