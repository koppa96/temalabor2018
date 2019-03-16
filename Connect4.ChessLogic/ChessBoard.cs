using System;
using System.Collections.Generic;
using System.Text;
using Connect4.Abstractions;
using Connect4.ChessLogic.Pieces;
using Connect4.Entities;

namespace Connect4.ChessLogic
{
    public class ChessBoard : ISerializableBoard<SerializedChessBoard>
    {
        private const int ChessboardSize = 8;
        private readonly List<Piece> pieces;
        private Field[,] board;

        public Field this[int row, int col] => board[row, col];

        public ChessBoard(bool newBoard)
        {
            board = new Field[ChessboardSize, ChessboardSize];
            pieces = new List<Piece>();

            for (int i = 0; i < ChessboardSize; i++)
            {
                for (int j = 0; j < ChessboardSize; j++)
                {
                    board[i, j] = new Field(i, j);
                }
            }

            if (newBoard)
            {
                InitializeBoard();
            }
        }

        private void InitializeBoard()
        {
            InitializePlayer(Color.White);
            InitializePlayer(Color.Black);
        }

        private void InitializePlayer(Color color)
        {
            int firstRow = 1, secondRow = 0;
            if (color == Color.White)
            {
                firstRow = 6;
                secondRow = 7;
            }

            Piece piece = new Rook(this, color);
            piece.AddToField(board[secondRow, 0]);

            piece = new Rook(this, color);
            piece.AddToField(board[secondRow, 7]);

            piece = new Knight(this, color);
            piece.AddToField(board[secondRow, 1]);

            piece = new Knight(this, color);
            piece.AddToField(board[secondRow, 6]);

            piece = new Bishop(this, color);
            piece.AddToField(board[secondRow, 2]);

            piece = new Bishop(this, color);
            piece.AddToField(board[secondRow, 5]);

            piece = new Queen(this, color);
            piece.AddToField(board[secondRow, 3]);
           
            piece = new King(this, color);
            piece.AddToField(board[secondRow, 4]);

            for (int i = 0; i < ChessboardSize; i++)
            {
                piece = new Pawn(this, color);
                piece.AddToField(board[firstRow, i]);
            }
        }

        public SerializedChessBoard ToSerialized()
        {
            throw new NotImplementedException();
        }

        public void FillFromSerialized(SerializedChessBoard serializedBoard)
        {
            throw new NotImplementedException();
        }

        public void RemovePiece(Piece piece)
        {
            pieces.Remove(piece);
        }

        private delegate void IncrementDelegate(ref int i, ref int j);

        public bool RouteClear(Field from, Field to)
        {
            Direction direction = Direction.GuessDirection(from, to);

            int i = from.Row + direction.RowDirection, j = from.Column + direction.ColumnDirection;

            while (direction.Predicate(i, j, to))
            {
                if (!board[i, j].Empty)
                {
                    return false;
                }

                i += direction.RowDirection;
                j += direction.ColumnDirection;
            }

            if (!(i == to.Row && j == to.Column))
            {
                throw new ArgumentException("The given fields are not in the given relative position from each other");
            }

            return true;
        }

        public void AddPieceToTheGame(Piece piece, Field field)
        {
            piece.AddToField(field);
            pieces.Add(piece);
        }
    }
}
