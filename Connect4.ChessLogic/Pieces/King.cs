using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4.ChessLogic.Pieces
{
    public class King : Piece
    {
        public King(ChessBoard board, Color color) : base(board, color)
        {
        }

        public override bool Move(Field targetField)
        {
            if (!base.Move(targetField))
            {
                return false;
            }

            if (Math.Abs(targetField.Row - Field.Row) < 2 && Math.Abs(targetField.Column - Field.Column) < 2)
            {
                SwitchPosition(targetField);
                return true;
            }

            return false;
        }
    }
}
