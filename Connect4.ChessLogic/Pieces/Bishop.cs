using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4.ChessLogic.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(ChessBoard board, Color color) : base(board, color)
        {
        }

        public override bool Move(Field targetField)
        {
            if (!base.Move(targetField))
            {
                return false;
            }

            if (Math.Abs(targetField.Row - Field.Row) == Math.Abs(targetField.Column - Field.Column) && Board.RouteClear(Field, targetField))
            {
                SwitchPosition(targetField);
                return true;
            }

            return false;
        }
    }
}
