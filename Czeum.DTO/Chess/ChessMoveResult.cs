using System;
using System.Collections.Generic;
using System.Text;
using Connect4.Abstractions;

namespace Czeum.DTO.Chess
{
    public class ChessMoveResult : MoveResult
    {
        public List<PieceInfo> PieceInfos { get; set; }
    }
}
