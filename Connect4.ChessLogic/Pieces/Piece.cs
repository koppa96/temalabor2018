using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Czeum.DTO.Chess;

namespace Connect4.ChessLogic.Pieces
{
    public abstract class Piece
    {
        protected ChessBoard Board { get; }
        public Color Color { get; }
        protected Field Field { get; set; }
        public abstract PieceInfo PieceInfo { get; }

        protected Piece(ChessBoard board, Color color)
        {
            Board = board;
            Color = color;
        }

        public virtual bool Move(Field targetField)
        {
            if (CanMoveTo(targetField))
            {
                SwitchPosition(targetField);
                return true;
            }

            return false;
        }

        public virtual bool CanMoveTo(Field targetField)
        {
            if (!targetField.Empty && targetField.Piece.Color == Color)
            {
                return false;
            }

            return true;
        }

        public virtual bool CanAttack(Field targetField)
        {
            return CanMoveTo(targetField);
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

        protected void SwitchPosition(Field to)
        {
            Field.RemovePiece(this);
            AddToField(to);
        }

        public override string ToString()
        {
            return Color.ToString()[0].ToString() + GetType().Name[0] + "_" + Field.ToString();
        }

        public static Piece CreateFromString(ChessBoard board, string str)
        {
            var color = str[0] == 'W' ? Color.White : Color.Black;

            switch (str[1])
            {
                case 'R':
                    return new Rook(board, color);
                case 'H':
                    return new Knight(board, color);
                case 'B':
                    return new Bishop(board, color);
                case 'Q':
                    return new Queen(board, color);
                case 'K':
                    return new King(board, color);
                case 'P':
                    return new Pawn(board, color);
            }

            return null;
        }
    }
}
