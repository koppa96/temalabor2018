using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;

namespace Czeum.Client.Interfaces {
    public interface ILobbyService {
        ObservableCollection<LobbyData> LobbyList { get; }
        LobbyData CurrentLobby { get; }
        void QueryLobbyList();
        void CreateLobby();
        void JoinLobby(int index);
        void LeaveLobby();
        ObservableCollection<LobbyData> GetLobbyList();
    }
}
