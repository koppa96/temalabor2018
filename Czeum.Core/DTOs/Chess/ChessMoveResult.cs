using System.Collections.Generic;
using Czeum.Core.DTOs.Abstractions;

namespace Czeum.Core.DTOs.Chess
{
    /// <summary>
    /// The MoveResult representation of a chess board.
    /// </summary>
    public class ChessMoveResult : IMoveResult
    {
        public bool WhiteKingInCheck { get; set; }
        public bool BlackKingInCheck { get; set; }
        public List<PieceInfo> PieceInfos { get; set; }
    }
}
