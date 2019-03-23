using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Czeum.Abstractions.DTO;

namespace Czeum.Server.Services.Lobby
{
    public class LobbyStorage : ILobbyStorage
    {
        private readonly ConcurrentDictionary<int, LobbyData> lobbies;

        public LobbyStorage()
        {
            lobbies = new ConcurrentDictionary<int, LobbyData>();
        }
        
        public IEnumerable<LobbyData> GetLobbies()
        {
            return lobbies.Values;
        }

        public LobbyData GetLobby(int lobbyId)
        {
            return lobbies[lobbyId];
        }

        public void AddLobby(LobbyData lobbyData)
        {           
            lobbyData.LobbyId = lobbies.Values.Max(l => l.LobbyId) + 1;
            lobbies[lobbyData.LobbyId] = lobbyData;
        }

        public void RemoveLobby(int lobbyId)
        {
            LobbyData removedLobby;
            lobbies.TryRemove(lobbyId, out removedLobby);
        }

        public void UpdateLobby(LobbyData lobbyData)
        {
            if (lobbies.ContainsKey(lobbyData.LobbyId))
            {
                lobbies[lobbyData.LobbyId] = lobbyData;
            }
        }

        public LobbyData GetLobbyOfUser(string user)
        {
            return lobbies.Values.SingleOrDefault(l => l.Host == user || l.Guest == user);
        }
    }
}