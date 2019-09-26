using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.Application.Models;
using Czeum.DTO;

namespace Czeum.Application.Services.Lobby
{
    public class LobbyStorage : ILobbyStorage
    {
        private readonly ConcurrentDictionary<Guid, LobbyStorageElement> content;

        public LobbyStorage()
        {
            content = new ConcurrentDictionary<Guid, LobbyStorageElement>();
        }
        
        public IEnumerable<LobbyData> GetLobbies()
        {
            return content.Values.Select(x => x.LobbyData);
        }

        public LobbyData GetLobby(Guid lobbyId)
        {
            if (content.ContainsKey(lobbyId))
            {
                return content[lobbyId].LobbyData;
            }

            return null;
        }

        public void AddLobby(LobbyData lobbyData)
        {
            lobbyData.Id = Guid.NewGuid();
            content[lobbyData.Id] = new LobbyStorageElement
            {
                LobbyData = lobbyData,
                Messages = new List<Message>()
            };
        }

        public void RemoveLobby(Guid lobbyId)
        {
            content.TryRemove(lobbyId, out _);
        }

        public void UpdateLobby(LobbyData lobbyData)
        {
            if (content.ContainsKey(lobbyData.Id))
            {
                content[lobbyData.Id].LobbyData = lobbyData;
            }
        }

        public LobbyData GetLobbyOfUser(string user)
        {
            return content.Values
                .SingleOrDefault(x => x.LobbyData.Host == user || x.LobbyData.Guest == user)
                ?.LobbyData;
        }

        public void AddMessage(Guid lobbyId, Message message)
        {
            content[lobbyId].Messages.Add(message);
        }

        public List<Message> GetMessages(Guid lobbyId)
        {
            return content[lobbyId].Messages;
        }
    }
}