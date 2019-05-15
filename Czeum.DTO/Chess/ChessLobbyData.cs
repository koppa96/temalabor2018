using System.Collections.Generic;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;

namespace Czeum.DTO.Chess
{
    /// <summary>
    /// The Lobby that is used to create Chess matches.
    /// </summary>
    public class ChessLobbyData : LobbyData
    {
        public override string ValidateSettings()
        {
            return null;
        }
    }
}