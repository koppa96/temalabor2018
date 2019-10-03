using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;
using Czeum.Abstractions.GameServices.BoardConverter;
using Czeum.Domain.Entities;
using Czeum.DTO.Chess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Czeum.Domain.Entities.Boards;

namespace Czeum.ChessLogic.Services
{
    public class ChessBoardConverter : BoardConverter<SerializedChessBoard>
    {
        public override IMoveResult Convert(SerializedChessBoard serializedBoard)
        {
            var board = new ChessBoard(false);
            board.DeserializeContent(serializedBoard);

            return new ChessMoveResult
            {
                WhiteKingInCheck = !board.IsKingSafe(Color.White),
                BlackKingInCheck = !board.IsKingSafe(Color.Black),
                PieceInfos = board.GetPieceInfos()
            };
        }
    }
}
