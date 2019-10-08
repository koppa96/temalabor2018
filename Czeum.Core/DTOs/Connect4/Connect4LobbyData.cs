using Czeum.Core.DTOs.Abstractions.Lobbies;

namespace Czeum.Core.DTOs.Connect4
{
    /// <summary>
    /// A lobby that is used to create Connect4 games.
    /// </summary>
    public class Connect4LobbyData : LobbyData
    {
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }

        public Connect4LobbyData()
        {
            BoardWidth = 7;
            BoardHeight = 6;
        }

        public override int MinimumPlayerCount => 2;
        public override int MaximumPlayerCount => 2;

        public override bool ValidateSettings()
        {
            // The height should be between 4 - 10, the width between 4-15
            return BoardWidth > 3 && BoardWidth < 16 && BoardHeight > 3 && BoardWidth < 11;
        }
    }
}