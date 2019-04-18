using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.ClientCallback;

namespace Czeum.Client.Interfaces {
    public interface ILobbyService {  
        ObservableCollection<LobbyData> LobbyList { get; }
        LobbyData CurrentLobby { get; }

        Task JoinLobby(int index);
        Task LeaveLobby();
        Task InvitePlayer(string player);
        Task CreateLobby(Type type);
        Task QueryLobbyList();
        Task UpdateLobby(LobbyData lobbyData);
    }
}
