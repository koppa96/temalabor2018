using System;
using Czeum.Core.DTOs.Chess;

namespace Czeum.ChessLogic.Pieces
{
    public class King : Piece
    {
        public bool InCheck => Board.IsFieldSafe(Color, Field!);

        public override PieceInfo PieceInfo => new PieceInfo()
        {
            Type = PieceType.King,
            Color = Color,
            Row = Field?.Row ?? -1,
            Column = Field?.Column ?? -1
        };

        public King(ChessBoard board, Color color) : base(board, color)
        {
        }

        public override bool CanMoveTo(Field targetField)
        {
            if (!base.CanMoveTo(targetField))
            {
                return false;
            }

            var canMove = Math.Abs(targetField.Row - Field!.Row) < 2 && Math.Abs(targetField.Column - Field!.Column) < 2;

            return canMove;
        }
    }
}
