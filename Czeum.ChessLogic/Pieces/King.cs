using System;
using System.Collections.Generic;
using Czeum.ChessLogic.Extensions;
using Czeum.Core.DTOs.Chess;

namespace Czeum.ChessLogic.Pieces
{
    public class King : Piece
    {
        public bool InCheck => Board.IsFieldSafe(Color, Field!);

        public override PieceInfo PieceInfo => new PieceInfo()
        {
            Type = PieceType.King,
            Color = Color,
            Row = Field?.Row ?? -1,
            Column = Field?.Column ?? -1
        };

        public King(ChessBoard board, Color color) : base(board, color)
        {
        }

        public override bool CanMoveTo(Field targetField)
        {
            if (!base.CanMoveTo(targetField))
            {
                return false;
            }

            var canMove = Math.Abs(targetField.Row - Field!.Row) < 2 && Math.Abs(targetField.Column - Field!.Column) < 2;

            return canMove;
        }

        public override List<ChessMoveData> GetPossibleMoves()
        {
            var currentField = Field!;
            var possibleMoves = new List<ChessMoveData>();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    if (currentField.Row + i >= 0 && currentField.Row + i < 8 && currentField.Column + j >= 0 && currentField.Column + j < 8)
                    {
                        var targetField = Board[currentField.Row + i, currentField.Column + j];
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
            return possibleMoves;
        }
    }
}
