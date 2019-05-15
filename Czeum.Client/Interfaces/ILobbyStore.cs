using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;

namespace Czeum.Client.Interfaces {
    public interface ILobbyStore
    {
        Task AddLobby(LobbyData lobby);
        Task RemoveLobby(int lobbyId);
        Task UpdateLobby(LobbyData lobby);
        Task ClearLobbies();

        ObservableCollection<LobbyData> LobbyList { get; }
        LobbyData SelectedLobby { get; set; }
    }
}
