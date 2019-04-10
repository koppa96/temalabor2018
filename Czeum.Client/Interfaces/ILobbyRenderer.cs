using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;

namespace Czeum.Client.Interfaces {
    public interface ILobbyRenderer
    {
        void RenderLobby(LobbyData lobbyData);
    }
}
