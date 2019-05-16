using Czeum.Abstractions.DTO;
using Czeum.Client.Interfaces;
using Czeum.DTO.Connect4;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.DTO;
using Czeum.DTO.Chess;

namespace Czeum.Client.Services {
    class DummyLobbyService : ILobbyService
    {
        private ILobbyStore lobbyStore;

        public DummyLobbyService(ILobbyStore lobbyStore)
        {
            this.lobbyStore = lobbyStore;
        }

        public ObservableCollection<LobbyData> LobbyList
        {
            get => lobbyStore.LobbyList;
        }

        public LobbyData CurrentLobby { get;  set; }

        public async Task CreateLobby(Type type) {
            LobbyList.Add(new Connect4LobbyData());
        }

        public async Task JoinLobby(int index) {
            CurrentLobby = LobbyList[index];
        }

        public async Task LeaveLobby() {
            CurrentLobby = null;
        }

        public Task InvitePlayer(string player)
        {
            throw new NotImplementedException();
        }

        public async Task QueryLobbyList()
        {
            if (LobbyList.Count > 0)
            {
                return;
            }
            LobbyList.Clear();
            LobbyList.Add(new Connect4LobbyData() { Host = "host1", Access = LobbyAccess.Public, LobbyId = 0, Guest = "guest1", Name = "My Little Lobbyyy"});
            LobbyList.Add(new ChessLobbyData() { Host = "host2", Access = LobbyAccess.Public, LobbyId = 1, InvitedPlayers = {"M", "asd", "asdasd", "faf"}});
            LobbyList.Add(new Connect4LobbyData() { Host = "host3", Access = LobbyAccess.Public, LobbyId = 2 , Name = "Noone's invited"});
            LobbyList.Add(new Connect4LobbyData() { Host = "host4", Access = LobbyAccess.Public, LobbyId = 3, Guest = "guest4" });
            LobbyList.Add(new ChessLobbyData() { Host = "host5", Access = LobbyAccess.Public, LobbyId = 4, Guest = "guest5", Name = "F off"});
        }

        public Task UpdateLobby(LobbyData lobbyData)
        {
            throw new NotImplementedException();
        }

        public Task CreateMatch()
        {
            throw new NotImplementedException();
        }

        public Task KickGuest()
        {
            throw new NotImplementedException();
        }
    }
}
