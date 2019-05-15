using System.Collections.Generic;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;

namespace Czeum.DTO.Connect4
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

        public override string ValidateSettings()
        {
            if (BoardWidth < 4 || BoardWidth > 15 || BoardHeight < 4 || BoardHeight > 10)
            {
                return ErrorCodes.InvalidBoardSize;
            }

            return null;
        }
    }
}