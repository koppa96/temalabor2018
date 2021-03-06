﻿using System;
using System.Collections.Generic;
using Czeum.Core.DTOs.Chess;

namespace Czeum.ChessLogic.Pieces
{
    public class Bishop : StraightLineMover
    {
        public override PieceInfo PieceInfo => new PieceInfo
        {
            Type = PieceType.Bishop,
            Color = Color,
            Row = Field?.Row ?? -1,
            Column = Field?.Column ?? -1
        };

        public Bishop(ChessBoard board, Color color) : base(board, color)
        {
        }

        public override bool CanMoveTo(Field targetField)
        {
            if (!base.CanMoveTo(targetField))
            {
                return false;
            }

            return Math.Abs(targetField.Row - Field!.Row) == Math.Abs(targetField.Column - Field!.Column)
                   && Board.RouteClear(Field!, targetField);
        }

        public override List<ChessMoveData> GetPossibleMoves()
        {
            var directions = new List<Direction>
            {
                Direction.AboveLeft,
                Direction.AboveRight,
                Direction.BelowLeft,
                Direction.BelowRight
            };

            return GetPossibleMoves(directions);
        }
    }
}
