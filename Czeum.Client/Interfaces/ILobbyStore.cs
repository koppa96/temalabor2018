using Czeum.Core.DTOs.Abstractions.Lobbies;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Client.Interfaces {
    public interface ILobbyStore
    {
        Task AddLobby(LobbyData lobby);
        Task RemoveLobby(Guid lobbyId);
        Task UpdateLobby(LobbyData lobby);
        Task ClearLobbies();

        ObservableCollection<LobbyData> LobbyList { get; }
        LobbyData SelectedLobby { get; set; }
    }
}
