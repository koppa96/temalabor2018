using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Client.Interfaces {
    interface ILobbyService {
        ObservableCollection<Abstractions.DTO.LobbyData> LobbyList();

        void QueryLobbyList();
        void CreateLobby();
        void JoinLobby();
        void LeaveLobby();

    }
}
