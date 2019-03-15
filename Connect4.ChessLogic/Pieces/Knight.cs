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
            throw new NotImplementedException();
        }
    }
}
