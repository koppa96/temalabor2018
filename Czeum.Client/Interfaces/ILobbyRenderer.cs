using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Czeum.Abstractions.DTO;

namespace Czeum.Client.Interfaces {
    public interface ILobbyRenderer
    {
        Panel RenderLobby(LobbyData lobbyData);
    }
}
