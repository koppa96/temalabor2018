using Czeum.Core.DTOs.Abstractions.Lobbies;

namespace Czeum.Core.DTOs.Connect4
{
    public class Connect4LobbySettings
    {
        public LobbySettingsField<int> BoardHeight { get; set; }
        public LobbySettingsField<int> BoardWidth { get; set; }
    }
}