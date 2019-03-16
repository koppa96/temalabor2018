using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4.ChessLogic.Pieces
{
    public class Knight : Piece
    {
        public Knight(ChessBoard board, Color color) : base(board, color)
        {
        }

        public override bool Move(Field targetField)
        {
            if (!base.Move(targetField))
            {
                return false;
            }

            if (Math.Abs(targetField.Row - Field.Row) == 2 && Math.Abs(targetField.Column - Field.Column) == 1
                || Math.Abs(targetField.Column - Field.Column) == 2 && Math.Abs(targetField.Row - Field.Row) == 1)
            {
                SwitchPosition(targetField);
                return true;
            }

            return false;
        }
    }
}
