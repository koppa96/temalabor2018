using System;
using System.Collections.Generic;
using System.Text;
using Czeum.DTO.Chess;

namespace Connect4.ChessLogic.Pieces
{
    public class King : Piece
    {
        public bool InCheck => Board.IsFieldSafe(Color, Field);

        public override PieceInfo PieceInfo => new PieceInfo()
        {
            Type = PieceType.King,
            Color = Color,
            Row = Field.Row,
            Column = Field.Column
        };

        public King(ChessBoard board, Color color) : base(board, color)
        {
        }

        public override bool CanMoveTo(Field targetField)
        {
            if (!base.Move(targetField))
            {
                return false;
            }

            return Math.Abs(targetField.Row - Field.Row) < 2 && Math.Abs(targetField.Column - Field.Column) < 2 
                                                             && Board.IsFieldSafe(Color, targetField);
        }
    }
}
