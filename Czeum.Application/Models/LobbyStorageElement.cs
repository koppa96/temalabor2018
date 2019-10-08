using System.Collections.Generic;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Abstractions.Lobbies;

namespace Czeum.Application.Models
{
    public class LobbyStorageElement
    {
        public LobbyData LobbyData { get; set; }
        public List<Message> Messages { get; set; }

        public LobbyStorageElement(LobbyData lobbyData)
        {
            LobbyData = lobbyData;
            Messages = new List<Message>();
        }
    }
}