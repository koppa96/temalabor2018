using System;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;

namespace Czeum.DTO.Lobbies
{
    public class CreateLobbyDto
    {
        public GameType GameType { get; set; }
        public LobbyAccess LobbyAccess { get; set; }
        public string Name { get; set; }
    }
}