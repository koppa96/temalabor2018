using System.Collections.Generic;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.Abstractions.GameServices;

namespace Czeum.DTO.Chess
{
    /// <summary>
    /// The Lobby that is used to create Chess matches.
    /// </summary>
    public class ChessLobbyData : LobbyData
    {
        public override int MinimumPlayerCount => 2;
        public override int MaximumPlayerCount => 2;
        protected override bool ValidateSettings()
        {
            return true;
        }
    }
}