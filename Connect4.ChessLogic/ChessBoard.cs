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
                    board[i, j] = new Field(new Position(i, j));
                }
            }

            if (!newBoard)
            {
                InitializeBoard();
            }
        }

        private void InitializeBoard()
        {
       
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

        public bool RouteClear(Field from, Field to, Direction direction)
        {
            if (from == to)
            {
                throw new ArgumentException("The fields are the same.");
            }

            var incrementDelegate = SetIncrementDelegate(direction);
            var predicate = SetPredicate(direction);

            int i = from.Position.Row, j = from.Position.Column;
            incrementDelegate(ref i, ref j);


            while (predicate(i, j, to))
            {
                if (!board[i, j].Empty)
                {
                    return false;
                }

                incrementDelegate(ref i, ref j);
            }

            if (!(i == to.Position.Row && j == to.Position.Column))
            {
                throw new ArgumentException("The given fields are not in the given relative position from each other");
            }

            return true;
        }

        private Func<int, int, Field, bool> SetPredicate(Direction direction)
        {
            switch (direction)
            {
                case Direction.Above:
                    return (i, j, f) => i > f.Position.Row;
                case Direction.Below:
                    return (i, j, f) => i < f.Position.Row;
                case Direction.Right:
                    return (i, j, f) => j < f.Position.Column;
                case Direction.Left:
                    return (i, j, f) => j > f.Position.Column;
                case Direction.AboveLeft:
                    return (i, j, f) => i > f.Position.Row && j > f.Position.Column;
                case Direction.AboveRight:
                    return (i, j, f) => i > f.Position.Row && j < f.Position.Column;
                case Direction.BelowRight:
                    return (i, j, f) => i < f.Position.Row && j < f.Position.Column;
                case Direction.BelowLeft:
                    return (i, j, f) => i < f.Position.Row && j > f.Position.Column;
                default:
                    throw new ArgumentException("No such direction");
            }
        }

        private IncrementDelegate SetIncrementDelegate(Direction direction)
        {
            switch (direction)
            {
                case Direction.Above:
                    return (ref int i, ref int j) => i--;
                case Direction.Below:
                    return (ref int i, ref int j) => i++;
                case Direction.Right:
                    return (ref int i, ref int j) => j++;
                case Direction.Left:
                    return (ref int i, ref int j) => j--;
                case Direction.AboveLeft:
                    return (ref int i, ref int j) =>
                    {
                        i--;
                        j--;
                    };
                case Direction.AboveRight:
                    return (ref int i, ref int j) =>
                    {
                        i--;
                        j++;
                    };
                case Direction.BelowLeft:
                    return (ref int i, ref int j) =>
                    {
                        i++;
                        j--;
                    };
                case Direction.BelowRight:
                    return (ref int i, ref int j) =>
                    {
                        i++;
                        j++;
                    };
                default:
                    throw new ArgumentException("No such direction");
            }
        }

        public Field GetFieldByPosition(Position position)
        {
            return board[position.Row, position.Column];
        }
    }
}
