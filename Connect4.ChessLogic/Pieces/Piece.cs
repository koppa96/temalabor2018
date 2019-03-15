using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4.ChessLogic.Pieces
{
    public abstract class Piece
    {
        protected ChessBoard Board { get; }
        protected Color Color { get; }
        protected Field Field { get; set; }

        protected Piece(ChessBoard board, Color color)
        {
            Board = board;
            Color = color;
        }

        public virtual bool Move(Field targetField)
        {
            if (!targetField.Empty && targetField.Piece.Color == Color)
            {
                return false;
            }

            return true;
        }

        public void HitBy(Piece piece)
        {
            Field.RemovePiece(this);
            Field = null;
            Board?.RemovePiece(this);
        }

        public void AddToField(Field targetField)
        {
            Field = targetField;
            targetField.AddPiece(this);
        }
    }
}
