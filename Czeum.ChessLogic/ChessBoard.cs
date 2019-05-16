using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Czeum.Abstractions;
using Czeum.ChessLogic.Pieces;
using Czeum.DAL.Entities;
using Czeum.DTO.Chess;

namespace Czeum.ChessLogic
{
    public class ChessBoard : ISerializableBoard<SerializedChessBoard>
    {
        public const int ChessboardSize = 8;
        private readonly List<Piece> pieces;
        private Field[,] board;
        private Piece hitPiece;

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
            AddPieceToTheGame(piece, board[secondRow, 0]);

            piece = new Rook(this, color);
            AddPieceToTheGame(piece, board[secondRow, 7]);

            piece = new Knight(this, color);
            AddPieceToTheGame(piece, board[secondRow, 1]);

            piece = new Knight(this, color);
            AddPieceToTheGame(piece, board[secondRow, 6]);

            piece = new Bishop(this, color);
            AddPieceToTheGame(piece, board[secondRow, 2]);

            piece = new Bishop(this, color);
            AddPieceToTheGame(piece, board[secondRow, 5]);

            piece = new Queen(this, color);
            AddPieceToTheGame(piece, board[secondRow, 3]);
           
            piece = new King(this, color);
            AddPieceToTheGame(piece, board[secondRow, 4]);

            for (int i = 0; i < ChessboardSize; i++)
            {
                piece = new Pawn(this, color);
                AddPieceToTheGame(piece, board[firstRow, i]);
            }
        }

        public bool MovePiece(Field from, Field to)
        {
            return from.Piece.Move(to);
        }

        private void UndoMove(Field from, Field to)
        {
            to.Piece.UndoMove(from);
            if (hitPiece != null)
            {
                pieces.Add(hitPiece);
                to.AddPiece(hitPiece);
                hitPiece.Field = to;
                hitPiece = null;
            }
        }

        public SerializedChessBoard SerializeContent()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var piece in pieces)
            {
                builder.Append(piece.ToString() + " ");
            }

            return new SerializedChessBoard()
            {
                BoardData = builder.ToString()
            };
        }

        public void DeserializeContent(SerializedChessBoard serializedBoard)
        {
            var pieceInfos = serializedBoard.BoardData.Trim().Split(' ');

            foreach (var pieceInfo in pieceInfos)
            {
                var positions = pieceInfo.Split('_')[1].Split(',');
                var piece = Piece.CreateFromString(this, pieceInfo);
                int row = int.Parse(positions[0]), column = int.Parse(positions[1]);
                AddPieceToTheGame(piece, board[row, column]);
            }
        }

        public void RemovePiece(Piece piece)
        {
            pieces.Remove(piece);
            hitPiece = piece;
        }

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

        public bool IsFieldSafe(Color color, Field field)
        {
            return !pieces.Any(p => p.Color != color && p.CanAttack(field));
        }

        public List<PieceInfo> GetPieceInfos()
        {
            return pieces.Select(p => p.PieceInfo).ToList();
        }

        public bool IsKingSafe(Color color)
        {
            var king = (King) pieces.FirstOrDefault(p => p is King && p.Color == color);
            return king.InCheck;
        }

        public bool ValidateMove(ChessMoveData move, Color color)
        {
            return move.FromRow >= 0 && move.FromRow < ChessboardSize && move.FromColumn >= 0 &&
                   move.FromColumn < ChessboardSize &&
                   move.ToRow >= 0 && move.ToRow < ChessboardSize && move.ToColumn >= 0 &&
                   move.ToColumn < ChessboardSize &&
                   !board[move.FromRow, move.FromColumn].Empty &&
                   board[move.FromRow, move.FromColumn].Piece.Color == color;
        }

        public List<ChessMoveData> GetPossibleMovesFor(Color color)
        {
            var myPieces = pieces.Where(p => p.Color == color);
            var possibleMoves = new List<ChessMoveData>();
            foreach (var piece in myPieces)
            {
                foreach (var field in board)
                {
                    if (piece.CanMoveTo(field))
                    {
                        possibleMoves.Add(new ChessMoveData
                        {
                            FromRow = piece.Field.Row,
                            FromColumn = piece.Field.Column,
                            ToRow = field.Row,
                            ToColumn = field.Column
                        });
                    }
                }
            }

            return possibleMoves;
        }

        public bool Stalemate(Color color, List<ChessMoveData> possibleMoves)
        {
            return possibleMoves.Count == 0 && IsKingSafe(color);
        }

        public bool Checkmate(Color color, List<ChessMoveData> possibleMoves)
        {
            if (IsKingSafe(color))
            {
                return false;
            }

            hitPiece = null;
            foreach (var move in possibleMoves)
            {
                MovePiece(board[move.FromRow, move.FromColumn], board[move.ToRow, move.ToColumn]);

                if (IsKingSafe(color))
                {
                    UndoMove(board[move.FromRow, move.FromColumn], board[move.ToRow, move.ToColumn]);
                    return false;
                }

                UndoMove(board[move.FromRow, move.FromColumn], board[move.ToRow, move.ToColumn]);
            }

            return true;
        }
    }
}
