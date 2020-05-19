using Czeum.Core.DTOs.Chess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.ChessLogic.Extensions
{
    public static class ChessMoveCollectionExtensions
    {
        public static void AddPossibleMove(this ICollection<ChessMoveData> moves, Field from, Field to)
        {
            moves.Add(new ChessMoveData
            {
                FromRow = from.Row,
                FromColumn = from.Column,
                ToRow = to.Row,
                ToColumn = to.Column
            });
        }
    }
}
