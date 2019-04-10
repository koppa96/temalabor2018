using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.Client.Interfaces;

namespace Czeum.Client {
    [LobbyRenderer("Chess")]
    class ChessLobbyRenderer : ILobbyRenderer {
        public void RenderLobby(LobbyData lobbyData)
        {
            throw new NotImplementedException("Chess");
        }
    }
}
