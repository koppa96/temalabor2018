using System;
using Czeum.Core.DTOs.Chess;

namespace Czeum.ChessLogic.Pieces
{
    public class Knight : Piece
    {
        public override PieceInfo PieceInfo => new PieceInfo()
        {
            Type = PieceType.Knight,
            Color = Color,
            Row = Field.Row,
            Column = Field.Column
        };

        public Knight(ChessBoard board, Color color) : base(board, color)
        {
        }

        public override bool CanMoveTo(Field targetField)
        {
            if (!base.CanMoveTo(targetField))
            {
                return false;
            }

            return Math.Abs(targetField.Row - Field.Row) == 2 && Math.Abs(targetField.Column - Field.Column) == 1
                   || Math.Abs(targetField.Column - Field.Column) == 2 && Math.Abs(targetField.Row - Field.Row) == 1;
        }

        public override string ToString()
        {
            return Color.ToString()[0].ToString() + "H_" + Field.Row + "," + Field.Column;
        }
    }
}
