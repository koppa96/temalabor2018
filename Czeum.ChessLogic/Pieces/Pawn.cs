using System;
using System.Collections.Generic;
using System.Text;
using Czeum.DTO.Chess;

namespace Czeum.ChessLogic.Pieces
{
    public class Pawn : Piece
    {
        private bool hasMoved;

        public override PieceInfo PieceInfo => new PieceInfo
        {
            Type = PieceType.Pawn,
            Color = Color,
            Row = Field.Row,
            Column = Field.Column
        };

        public Pawn(ChessBoard board, Color color, bool hasMoved = false) : base(board, color)
        {
            this.hasMoved = hasMoved;
        }

        public override bool CanMoveTo(Field targetField)
        {
            if (!base.CanMoveTo(targetField))
            {
                return false;
            }

            var direction = Color == Color.White ? Direction.Above : Direction.Below;

            return targetField.Row - direction.RowDirection == Field.Row && targetField.Column == Field.Column && targetField.Empty 
                   || CanAttack(targetField) && !targetField.Empty
                   || targetField.Row - 2 * direction.RowDirection == Field.Row && targetField.Column == Field.Column && !hasMoved && Board.RouteClear(Field, targetField);
        }

        public override bool CanAttack(Field targetField)
        {
            switch (Color)
            {
                case Color.White when targetField.Row + 1 == Field.Row && Math.Abs(targetField.Column - Field.Column) == 1:
                case Color.Black when targetField.Row - 1 == Field.Row && Math.Abs(targetField.Column - Field.Column) == 1:
                    return true;
                default:
                    return false;
            }
        }

        public override bool Move(Field targetField)
        {
            var result = base.Move(targetField);

            if (result)
            {
                hasMoved = true;
            }

            return result;
        }

        internal override bool TestMove(Field targetField)
        {
            return base.Move(targetField);
        }

        public override string ToString()
        {
            return base.ToString() + "_" + (hasMoved ? 't' : 'f');
        }
    }
}
