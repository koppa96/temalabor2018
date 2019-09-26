using System.Collections.Generic;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.DTO;

namespace Czeum.Application.Models
{
    public class LobbyStorageElement
    {
        public LobbyData LobbyData { get; set; }
        public List<Message> Messages { get; set; }
    }
}