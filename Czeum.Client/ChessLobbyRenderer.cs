using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.Client.Interfaces;
using Czeum.DTO.Chess;

namespace Czeum.Client {
    [LobbyRenderer(typeof(ChessLobbyData))]
    class ChessLobbyRenderer : ILobbyRenderer {
        public void RenderLobby(LobbyData lobbyData)
        {
            throw new NotImplementedException("Chess");
        }
    }
}
