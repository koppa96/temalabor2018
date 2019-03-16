using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4.ChessLogic.Pieces
{
    public class Rook : Piece
    {
        public Rook(ChessBoard board, Color color) : base(board, color)
        {
        }

        public override bool Move(Field targetField)
        {
            if (!base.Move(targetField))
            {
                return false;
            }

            if (targetField.Row == Field.Row)
            {
                if (Board.RouteClear(Field, targetField))
                {
                    SwitchPosition(targetField);
                    return true;
                }
            }

            if (targetField.Column == Field.Column)
            {
                if (Board.RouteClear(Field, targetField))
                {
                    SwitchPosition(targetField);
                    return true;
                }
            }

            return false;
        }
    }
}
