using Czeum.Core.DTOs.Abstractions;

namespace Czeum.Core.DTOs.Connect4
{
    /// <summary>
    /// The MoveData of the Connect4 game.
    /// </summary>
    public class Connect4MoveData : MoveData
    {
        public int Column { get; set; }
    }
}
