using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Connect4.ChessLogic.Pieces;

namespace Connect4.ChessLogic
{
    public class Field
    {
        public bool Empty => Piece == null;
        public Piece Piece { get; private set; }
        public Position Position { get; }

        public Field(Position position)
        {
            Position = position;
        }

        public void AddPiece(Piece piece)
        {
            Piece?.HitBy(piece);
            Piece = piece;
        }

        public void RemovePiece(Piece piece)
        {
            if (Piece == piece)
            {
                Piece = null;
            }
        }
    }
}
