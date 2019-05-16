using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Czeum.DTO.Chess;

namespace Czeum.ChessLogic.Pieces
{
    public abstract class Piece
    {
        protected ChessBoard Board { get; }
        public Color Color { get; }
        public Field Field { get; set; }
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
                return SwitchPosition(targetField);
            }

            return false;
        }

        internal virtual bool TestMove(Field targetField)
        {
            return Move(targetField);
        }

        internal bool UndoMove(Field targetField)
        {
            return SwitchPosition(targetField);
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

        public bool HitBy(Piece piece)
        {
            if (piece.Color == Color)
            {
                return false;
            }

            Field.RemovePiece(this);
            Field = null;
            Board.RemovePiece(this);
            return true;
        }

        public bool AddToField(Field targetField)
        {
            if (targetField.AddPiece(this))
            {
                Field = targetField;
                return true;
            }

            return false;
        }

        protected bool SwitchPosition(Field to)
        {
            var oldField = Field;
            if (AddToField(to))
            {
                oldField.RemovePiece(this);
                return true;
            }

            return false;
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
                    return new Pawn(board, color, str.Last() == 't');
            }

            return null;
        }
    }
}
