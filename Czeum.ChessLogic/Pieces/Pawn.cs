using System;
using System.Collections.Generic;
using Czeum.ChessLogic.Extensions;
using Czeum.Core.DTOs.Chess;

namespace Czeum.ChessLogic.Pieces
{
    public class Pawn : Piece
    {
        private bool hasMoved;

        public override PieceInfo PieceInfo => new PieceInfo
        {
            Type = PieceType.Pawn,
            Color = Color,
            Row = Field?.Row ?? -1,
            Column = Field?.Column ?? -1
        };

        public Pawn(ChessBoard board, Color color, bool hasMoved = false) : base(board, color)
        {
            this.hasMoved = hasMoved;
        }

        public override bool CanMoveTo(Field targetField)
        {
            if (!base.CanMoveTo(targetField))
            {
                return false;
            }

            var direction = Color == Color.White ? Direction.Above : Direction.Below;

            return targetField.Row - direction.RowDirection == Field!.Row && targetField.Column == Field!.Column && targetField.Empty 
                   || CanAttack(targetField) && !targetField.Empty
                   || targetField.Row - 2 * direction.RowDirection == Field!.Row && targetField.Column == Field!.Column && !hasMoved && Board.RouteClear(Field!, targetField);
        }

        public override bool CanAttack(Field targetField)
        {
            if (!base.CanMoveTo(targetField))
            {
                return false;
            }
            
            switch (Color)
            {
                case Color.White when targetField.Row + 1 == Field!.Row && Math.Abs(targetField.Column - Field!.Column) == 1:
                case Color.Black when targetField.Row - 1 == Field!.Row && Math.Abs(targetField.Column - Field!.Column) == 1:
                    return true;
                default:
                    return false;
            }
        }

        public override bool Move(Field targetField)
        {
            var result = base.Move(targetField);

            if (result)
            {
                hasMoved = true;
            }

            return result;
        }

        internal override bool TestMove(Field targetField)
        {
            return base.Move(targetField);
        }

        public override string ToString()
        {
            return base.ToString() + "_" + (hasMoved ? 't' : 'f');
        }

        public override List<ChessMoveData> GetPossibleMoves()
        {
            var possibleMoves = new List<ChessMoveData>();
            var currentField = Field!;
            if (Color == Color.Black)
            {
                if (currentField.Row < 7)
                {
                    var fieldBelow = Board[currentField.Row + 1, currentField.Column];
                    if (fieldBelow.Empty)
                    {
                        Board.TestMovePiece(currentField, fieldBelow);
                        if (Board.IsKingSafe(Color))
                        {
                            possibleMoves.AddPossibleMove(currentField, fieldBelow);
                        }
                        Board.UndoMove(currentField, fieldBelow);
                    }

                    if (currentField.Column > 0)
                    {
                        var fieldBelowLeft = Board[currentField.Row + 1, currentField.Column - 1];
                        if (!fieldBelowLeft.Empty && fieldBelowLeft.Piece!.Color != Color)
                        {
                            Board.TestMovePiece(currentField, fieldBelowLeft);
                            if (Board.IsKingSafe(Color))
                            {
                                possibleMoves.AddPossibleMove(currentField, fieldBelowLeft);
                            }
                            Board.UndoMove(currentField, fieldBelowLeft);
                        }
                    }

                    if (currentField.Column < 7)
                    {
                        var fieldBelowRight = Board[currentField.Row + 1, currentField.Column + 1];
                        if (!fieldBelowRight.Empty && fieldBelowRight.Piece!.Color != Color)
                        {
                            Board.TestMovePiece(currentField, fieldBelowRight);
                            if (Board.IsKingSafe(Color))
                            {
                                possibleMoves.AddPossibleMove(currentField, fieldBelowRight);
                            }
                            Board.UndoMove(currentField, fieldBelowRight);
                        }
                    }

                    if (!hasMoved)
                    {
                        var fieldBelowBy2 = Board[currentField.Row + 2, currentField.Column];
                        if (fieldBelowBy2.Empty)
                        {
                            Board.TestMovePiece(currentField, fieldBelowBy2);
                            if (Board.IsKingSafe(Color))
                            {
                                possibleMoves.AddPossibleMove(currentField, fieldBelowBy2);
                            }
                            Board.UndoMove(currentField, fieldBelowBy2);
                        }
                    }
                }
            }
            else
            {
                if (currentField.Row > 0)
                {
                    var fieldAbove = Board[currentField.Row - 1, currentField.Column];
                    if (fieldAbove.Empty)
                    {
                        Board.TestMovePiece(currentField, fieldAbove);
                        if (Board.IsKingSafe(Color))
                        {
                            possibleMoves.AddPossibleMove(currentField, fieldAbove);
                        }
                        Board.UndoMove(currentField, fieldAbove);
                    }

                    if (currentField.Column > 0)
                    {
                        var fieldAboveLeft = Board[currentField.Row - 1, currentField.Column - 1];
                        if (!fieldAboveLeft.Empty && fieldAboveLeft.Piece!.Color != Color)
                        {
                            Board.TestMovePiece(currentField, fieldAboveLeft);
                            if (Board.IsKingSafe(Color))
                            {
                                possibleMoves.AddPossibleMove(currentField, fieldAboveLeft);
                            }
                            Board.UndoMove(currentField, fieldAboveLeft);
                        }
                    }

                    if (currentField.Column < 7)
                    {
                        var fieldAboveRight = Board[currentField.Row - 1, currentField.Column + 1];
                        if (!fieldAboveRight.Empty && fieldAboveRight.Piece!.Color != Color)
                        {
                            Board.TestMovePiece(currentField, fieldAboveRight);
                            if (Board.IsKingSafe(Color))
                            {
                                possibleMoves.AddPossibleMove(currentField, fieldAboveRight);
                            }
                            Board.UndoMove(currentField, fieldAboveRight);
                        }
                    }

                    if (!hasMoved)
                    {
                        var fieldAboveBy2 = Board[currentField.Row - 2, currentField.Column];
                        if (fieldAboveBy2.Empty)
                        {
                            Board.TestMovePiece(currentField, fieldAboveBy2);
                            if (Board.IsKingSafe(Color))
                            {
                                possibleMoves.AddPossibleMove(currentField, fieldAboveBy2);
                            }
                            Board.UndoMove(currentField, fieldAboveBy2);
                        }
                    }
                }
            }

            return possibleMoves;
        }
    }
}
