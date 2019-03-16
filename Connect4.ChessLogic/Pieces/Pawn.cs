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
                                           to + 2 == from && !hasMoved && Board.RouteClear(Field, targetField));
                }
                else
                {
                    return MovePawnForward(targetField, (from, to) => to - 1 == from ||
                                           to - 2 == from && !hasMoved && Board.RouteClear(Field, targetField));
                }
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        private bool MovePawnForward(Field to, Func<int, int, bool> rule)
        {
            if (!rule(Field.Row, to.Row))
            {
                return false;
            }

            if (Field.Column == to.Column && to.Empty || 
                Math.Abs(Field.Column - to.Column) == 1 && !to.Empty && Math.Abs(to.Row - Field.Row) == 1)
            {
                SwitchPosition(to);
                hasMoved = true;
                return true;
            }

            return false;
        }
    }
}
