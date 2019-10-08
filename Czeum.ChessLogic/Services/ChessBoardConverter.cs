using Czeum.Core.DTOs.Abstractions;
using Czeum.Core.DTOs.Chess;
using Czeum.Core.GameServices.BoardConverter;
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
