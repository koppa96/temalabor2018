using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4.ChessLogic.Pieces
{
    public class Pawn : Piece
    {
        private bool hasMoved;

        public Pawn(ChessBoard board, Color color) : base(board, color)
        {
            hasMoved = false;
        }

        public override bool Move(Field targetField)
        {
            if (!base.Move(targetField))
            {
                return false;
            }

            try
            {
                if (Color == Color.White)
                {
                    return MovePawnForward(targetField, (from, to) => to + 1 == from ||
                                           to + 2 == from && !hasMoved && Board.RouteClear(Field, targetField, Direction.Above));
                }
                else
                {
                    return MovePawnForward(targetField, (from, to) => to - 1 == from ||
                                           to - 2 == from && !hasMoved && Board.RouteClear(Field, targetField, Direction.Below));
                }
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        private bool MovePawnForward(Field to, Func<int, int, bool> rule)
        {
            if (!rule(Field.Position.Row, to.Position.Row))
            {
                return false;
            }

            if (Field.Position.Column == to.Position.Column && to.Empty || 
                Math.Abs(Field.Position.Column - to.Position.Column) == 1 && !to.Empty 
                                                                          && Math.Abs(to.Position.Row - Field.Position.Row) == 1)
            {
                Field.RemovePiece(this);
                Field = to;
                to.AddPiece(this);

                return true;
            }

            hasMoved = true;
            return false;
        }
    }
}
