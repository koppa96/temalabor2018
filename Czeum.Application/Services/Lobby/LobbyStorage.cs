using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.DTO;

namespace Czeum.Application.Services.Lobby
{
    public class LobbyStorage : ILobbyStorage
    {
        private readonly ConcurrentDictionary<int, LobbyData> lobbies;
        private readonly ConcurrentDictionary<int, List<Message>> messages;

        public LobbyStorage()
        {
            lobbies = new ConcurrentDictionary<int, LobbyData>();
            messages = new ConcurrentDictionary<int, List<Message>>();
        }
        
        public IEnumerable<LobbyData> GetLobbies()
        {
            return lobbies.Values;
        }

        public LobbyData GetLobby(int lobbyId)
        {
            if (lobbies.ContainsKey(lobbyId))
            {
                return lobbies[lobbyId];
            }

            return null;
        }

        public void AddLobby(LobbyData lobbyData)
        {
            int newLobbyId = 1;
            while (lobbies.ContainsKey(newLobbyId))
            {
                newLobbyId++;
            }

            lobbies[lobbyData.LobbyId] = lobbyData;
            messages[lobbyData.LobbyId] = new List<Message>();
        }

        public void RemoveLobby(int lobbyId)
        {
            lobbies.TryRemove(lobbyId, out _);
            messages.TryRemove(lobbyId, out _);
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

        public void AddMessage(int lobbyId, Message message)
        {
            messages[lobbyId].Add(message);
        }

        public List<Message> GetMessages(int lobbyId)
        {
            return messages[lobbyId];
        }
    }
}