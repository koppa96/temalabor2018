using Czeum.Core.Enums;

namespace Czeum.Core.DTOs.Lobbies
{
    public class CreateLobbyDto
    {
        public GameType GameType { get; set; }
        public LobbyAccess LobbyAccess { get; set; }
        public string Name { get; set; }
    }
}