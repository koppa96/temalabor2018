using Czeum.Abstractions.DTO;
using Czeum.Client.Interfaces;
using Czeum.DTO.Connect4;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Client.Services {
    class DummyLobbyService : ILobbyService {

        private ObservableCollection<LobbyData> _lobbyListList;
        private LobbyData _currentLobby;

        public DummyLobbyService() {
            _lobbyListList = new ObservableCollection<LobbyData>();
        }

        public ObservableCollection<LobbyData> LobbyList => _lobbyListList;
        public LobbyData CurrentLobby => _currentLobby;

        public void CreateLobby() {
            _lobbyListList.Add(new Connect4LobbyData());
        }

        public ObservableCollection<LobbyData> GetLobbyList() {
            return _lobbyListList;
        }

        public void JoinLobby(int index) {
            _currentLobby = _lobbyListList[index];
        }

        public void LeaveLobby() {
            _currentLobby = null;
        }

        public void QueryLobbyList() {
            _lobbyListList.Add(new Connect4LobbyData() { Host = "host1", Access = LobbyAccess.Public, LobbyId = 0 });
            _lobbyListList.Add(new Connect4LobbyData() { Host = "host2", Access = LobbyAccess.Public, LobbyId = 1 });
            _lobbyListList.Add(new Connect4LobbyData() { Host = "host3", Access = LobbyAccess.Public, LobbyId = 2 });
            _lobbyListList.Add(new Connect4LobbyData() { Host = "host4", Access = LobbyAccess.Public, LobbyId = 3 });
            _lobbyListList.Add(new Connect4LobbyData() { Host = "host5", Access = LobbyAccess.Public, LobbyId = 4 });
        }
    }
}
