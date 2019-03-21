using System;
using System.Collections.Generic;
using System.Text;
using Czeum.DTO.Chess;

namespace Czeum.ChessLogic.Pieces
{
    public class Bishop : Piece
    {
        public override PieceInfo PieceInfo => new PieceInfo()
        {
            Type = PieceType.Bishop,
            Color = Color,
            Row = Field.Row,
            Column = Field.Column
        };

        public Bishop(ChessBoard board, Color color) : base(board, color)
        {
        }

        public override bool CanMoveTo(Field targetField)
        {
            if (!base.CanMoveTo(targetField))
            {
                return false;
            }

            return Math.Abs(targetField.Row - Field.Row) == Math.Abs(targetField.Column - Field.Column)
                   && Board.RouteClear(Field, targetField);
        }
    }
}
