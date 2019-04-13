using Czeum.Abstractions.DTO;
using Czeum.Client.Interfaces;
using Czeum.DTO.Connect4;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.DTO.Chess;

namespace Czeum.Client.Services {
    class DummyLobbyService : ILobbyService {

        private ObservableCollection<LobbyData> _lobbyList;
        private LobbyData _currentLobby;

        public DummyLobbyService() {
            _lobbyList = new ObservableCollection<LobbyData>();
        }

        public ObservableCollection<LobbyData> LobbyList => _lobbyList;
        public LobbyData CurrentLobby => _currentLobby;

        public void CreateLobby() {
            _lobbyList.Add(new Connect4LobbyData());
        }

        public ObservableCollection<LobbyData> GetLobbyList() {
            return _lobbyList;
        }

        public void JoinLobby(int index) {
            _currentLobby = _lobbyList[index];
        }

        public void LeaveLobby() {
            _currentLobby = null;
        }

        public void QueryLobbyList()
        {
            if (LobbyList.Count > 0)
            {
                return;
            }
            _lobbyList.Clear();
            _lobbyList.Add(new Connect4LobbyData() { Host = "host1", Access = LobbyAccess.Public, LobbyId = 0, Guest = "guest1", Name = "My Little Lobbyyy"});
            _lobbyList.Add(new ChessLobbyData() { Host = "host2", Access = LobbyAccess.Public, LobbyId = 1, InvitedPlayers = {"M", "asd", "asdasd", "faf"}});
            _lobbyList.Add(new Connect4LobbyData() { Host = "host3", Access = LobbyAccess.Public, LobbyId = 2 , Name = "Noone's invited"});
            _lobbyList.Add(new Connect4LobbyData() { Host = "host4", Access = LobbyAccess.Public, LobbyId = 3, Guest = "guest4" });
            _lobbyList.Add(new ChessLobbyData() { Host = "host5", Access = LobbyAccess.Public, LobbyId = 4, Guest = "guest5", Name = "F off"});
        }
    }
}
