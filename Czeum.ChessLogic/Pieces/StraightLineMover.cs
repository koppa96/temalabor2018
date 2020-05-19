using Czeum.ChessLogic.Extensions;
using Czeum.Core.DTOs.Chess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.ChessLogic.Pieces
{
    public abstract class StraightLineMover : Piece
    {
        protected StraightLineMover(ChessBoard board, Color color) : base(board, color)
        {
        }

        protected List<ChessMoveData> GetPossibleMoves(List<Direction> directions)
        {
            var currentField = Field!;
            var possibleMoves = new List<ChessMoveData>();
            foreach (var direction in directions)
            {
                var row = currentField!.Row + direction.RowDirection;
                var column = currentField!.Column + direction.ColumnDirection;

                while (row >= 0 && row < 8 && column >= 0 && column < 8)
                {
                    var field = Board[row, column];
                    if (!field.Empty)
                    {
                        if (field.Piece!.Color == Color)
                        {
                            break;
                        }
                        else
                        {
                            Board.TestMovePiece(currentField, field);
                            if (Board.IsKingSafe(Color))
                            {
                                possibleMoves.AddPossibleMove(currentField, field);
                            }
                            Board.UndoMove(currentField, field);
                            break;
                        }
                    }
                    else
                    {
                        Board.TestMovePiece(currentField, field);
                        if (Board.IsKingSafe(Color))
                        {
                            possibleMoves.AddPossibleMove(currentField, field);
                        }
                        Board.UndoMove(currentField, field);
                    }

                    row += direction.RowDirection;
                    column += direction.ColumnDirection;
                }
            }

            return possibleMoves;
        }
    }
}
