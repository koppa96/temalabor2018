using Czeum.Core.DTOs.Abstractions.Lobbies;

namespace Czeum.Core.DTOs.Chess
{
    /// <summary>
    /// The Lobby that is used to create Chess matches.
    /// </summary>
    public class ChessLobbyData : LobbyData
    {
        public override int MinimumPlayerCount => 2;
        public override int MaximumPlayerCount => 2;
        public override bool ValidateSettings()
        {
            return true;
        }
    }
}