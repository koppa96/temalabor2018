using System;
using System.Collections.Generic;
using System.Text;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;

namespace Czeum.DTO.Chess
{
    /// <summary>
    /// The MoveResult representation of a chess board.
    /// </summary>
    public class ChessMoveResult : MoveResult
    {
        public bool WhiteKingInCheck { get; set; }
        public bool BlackKingInCheck { get; set; }
        public List<PieceInfo> PieceInfos { get; set; }
    }
}
