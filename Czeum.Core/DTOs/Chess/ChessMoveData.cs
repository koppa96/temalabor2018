using Czeum.Core.DTOs.Abstractions;

namespace Czeum.Core.DTOs.Chess
{
    /// <summary>
    /// The MoveData of the chess game.
    /// </summary>
    public class ChessMoveData : MoveData
    {
        public int FromRow { get; set; }
        public int FromColumn { get; set; }
        public int ToRow { get; set; }
        public int ToColumn { get; set; }
    }
}
