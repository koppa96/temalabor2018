using Czeum.ChessLogic.Pieces;

namespace Czeum.ChessLogic
{
    public class Field
    {
        public bool Empty => Piece == null;
        public Piece Piece { get; private set; }
        public int Row { get; }
        public int Column { get; }

        public Field(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public bool AddPiece(Piece piece)
        {
            if (Piece == null || Piece.HitBy(piece))
            {
                Piece = piece;
                return true;
            }

            return false;
        }

        public void RemovePiece(Piece piece)
        {
            if (Piece == piece)
            {
                Piece = null;
            }
        }

        public override string ToString()
        {
            return Row + "," + Column;
        }
    }
}
