using System;
using System.Collections.Generic;
using System.Data;
using Czeum.ChessLogic.Extensions;
using Czeum.Core.DTOs.Chess;

namespace Czeum.ChessLogic.Pieces
{
    public class Knight : Piece
    {
        public override PieceInfo PieceInfo => new PieceInfo()
        {
            Type = PieceType.Knight,
            Color = Color,
            Row = Field?.Row ?? -1,
            Column = Field?.Column ?? -1
        };

        public Knight(ChessBoard board, Color color) : base(board, color)
        {
        }

        public override bool CanMoveTo(Field targetField)
        {
            if (!base.CanMoveTo(targetField))
            {
                return false;
            }

            return Math.Abs(targetField.Row - Field!.Row) == 2 && Math.Abs(targetField.Column - Field!.Column) == 1
                   || Math.Abs(targetField.Column - Field!.Column) == 2 && Math.Abs(targetField.Row - Field!.Row) == 1;
        }

        public override string ToString()
        {
            return Color.ToString()[0] + "H_" + (Field?.Row ?? -1) + "," + (Field?.Column ?? -1);
        }

        public override List<ChessMoveData> GetPossibleMoves()
        {
            var currentField = Field!;
            var possibleMoves = new List<ChessMoveData>();
            for (int i = -2; i < 3; i++)
            {
                if (i == 0)
                {
                    continue;
                }
                else if (Math.Abs(i) == 2)
                {
                    if (currentField.Row + i >= 0 && currentField.Row + i < 8)
                    {
                        if (currentField.Column > 0)
                        {
                            var targetField = Board[currentField.Row + i, currentField.Column - 1];
                            if (targetField.Empty || targetField.Piece!.Color != Color)
                            {
                                Board.TestMovePiece(currentField, targetField);
                                if (Board.IsKingSafe(Color))
                                {
                                    possibleMoves.AddPossibleMove(currentField, targetField);
                                }
                                Board.UndoMove(currentField, targetField);
                            }
                        }
                        
                        if (currentField.Column < 7)
                        {
                            var targetField = Board[currentField.Row + i, currentField.Column + 1];
                            if (targetField.Empty || targetField.Piece!.Color != Color)
                            {
                                Board.TestMovePiece(currentField, targetField);
                                if (Board.IsKingSafe(Color))
                                {
                                    possibleMoves.AddPossibleMove(currentField, targetField);
                                }
                                Board.UndoMove(currentField, targetField);
                            }
                        }
                    }
                }
                else
                {
                    if (currentField.Row + i >= 0 && currentField.Row + i < 8)
                    {
                        if (currentField.Column > 1)
                        {
                            var targetField = Board[currentField.Row + i, currentField.Column - 2];
                            if (targetField.Empty || targetField.Piece!.Color != Color)
                            {
                                Board.TestMovePiece(currentField, targetField);
                                if (Board.IsKingSafe(Color))
                                {
                                    possibleMoves.AddPossibleMove(currentField, targetField);
                                }
                                Board.UndoMove(currentField, targetField);
                            }
                        }

                        if (currentField.Column < 6)
                        {
                            var targetField = Board[currentField.Row + i, currentField.Column + 2];
                            if (targetField.Empty || targetField.Piece!.Color != Color)
                            {
                                Board.TestMovePiece(currentField, targetField);
                                if (Board.IsKingSafe(Color))
                                {
                                    possibleMoves.AddPossibleMove(currentField, targetField);
                                }
                                Board.UndoMove(currentField, targetField);
                            }
                        }
                    }
                }
            }
            return possibleMoves;
        }
    }
}
