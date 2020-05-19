using Czeum.Core.DTOs.Chess;
using System.Collections.Generic;

namespace Czeum.ChessLogic.Pieces
{
    public class Rook : StraightLineMover
    {
        public override PieceInfo PieceInfo => new PieceInfo()
        {
            Type = PieceType.Rook,
            Color = Color,
            Row = Field?.Row ?? -1,
            Column = Field?.Column ?? -1
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

            return targetField.Row == Field!.Row && Board.RouteClear(Field!, targetField) ||
                   targetField.Column == Field!.Column && Board.RouteClear(Field!, targetField);
        }

        public override List<ChessMoveData> GetPossibleMoves()
        {
            var directions = new List<Direction>
            {
                Direction.Above,
                Direction.Right,
                Direction.Below,
                Direction.Left
            };

            return GetPossibleMoves(directions);
        }
    }
}
