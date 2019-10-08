using Czeum.Core.Domain;
using Czeum.Core.DTOs.Chess;
using Czeum.Core.GameServices.BoardCreator;

namespace Czeum.ChessLogic.Services
{
    public class ChessBoardCreator : BoardCreator<ChessLobbyData>
    {
        public override SerializedBoard CreateBoard(ChessLobbyData lobbyData)
        {
            return CreateDefaultBoard();
        }

        public override SerializedBoard CreateDefaultBoard()
        {
            return new ChessBoard(true).SerializeContent();
        }
    }
}
