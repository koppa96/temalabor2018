using System;
using System.Collections.Generic;
using System.Text;
using Czeum.DTO.Chess;

namespace Connect4.ChessLogic.Pieces
{
    public class Rook : Piece
    {
        public override PieceInfo PieceInfo => new PieceInfo()
        {
            Type = PieceType.Rook,
            Color = Color,
            Row = Field.Row,
            Column = Field.Column
        };

        public Rook(ChessBoard board, Color color) : base(board, color)
        {
        }

        public override bool CanMoveTo(Field targetField)
        {
            if (!base.CanMoveTo(targetField))
            {
                return false;
            }

            return targetField.Row == Field.Row && Board.RouteClear(Field, targetField) ||
                   targetField.Column == Field.Column && Board.RouteClear(Field, targetField);
        }
    }
}
