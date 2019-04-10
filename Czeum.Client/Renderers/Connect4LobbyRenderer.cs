using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.Client.Interfaces;
using Czeum.DTO.Connect4;

namespace Czeum.Client {
    [LobbyRenderer(typeof(Connect4LobbyData))]
    class Connect4LobbyRenderer : ILobbyRenderer{
        public void RenderLobby(LobbyData lobbyData)
        {
            throw new NotImplementedException("Connect4");
        }
    }
}
