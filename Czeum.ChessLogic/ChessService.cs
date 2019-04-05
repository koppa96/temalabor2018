using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;
using Czeum.DAL.Entities;
using Czeum.DAL.Interfaces;
using Czeum.DTO.Chess;

namespace Czeum.ChessLogic
{
    [GameService(typeof(ChessMoveData), typeof(ChessLobbyData))]
    public class ChessService : IGameService
    {
        private readonly IBoardRepository<SerializedChessBoard> _repository;

        public ChessService(IBoardRepository<SerializedChessBoard> repository)
        {
            _repository = repository;
        }

        public MoveResult ExecuteMove(MoveData moveData, int playerId)
        {
            var move = (ChessMoveData) moveData;
            var color = playerId == 1 ? Color.White : Color.Black;
            var enemyColor = playerId == 1 ? Color.Black : Color.White;

            var serializedBoard = _repository.GetByMatchId(move.MatchId);
            var board = new ChessBoard(false);
            board.DeserializeContent(serializedBoard);

            var oldBoard = board.GetPieceInfos();
            if (!board.ValidateMove(move, color) || 
                !board.MovePiece(board[move.FromRow, move.FromColumn], board[move.ToRow, move.ToColumn]) ||
                !board.IsKingSafe(color))
            {
                return new ChessMoveResult
                {
                    Status = Status.Fail,
                    PieceInfos = oldBoard
                };
            }

            _repository.UpdateBoardData(serializedBoard.BoardId, board.SerializeContent().BoardData);
            var enemyMoves = board.GetPossibleMovesFor(enemyColor);
            if (board.Stalemate(enemyColor, enemyMoves))
            {
                return new ChessMoveResult
                {
                    Status = Status.Draw,
                    PieceInfos = board.GetPieceInfos()
                };
            }

            if (board.Checkmate(enemyColor, enemyMoves))
            {
                return new ChessMoveResult
                {
                    Status = Status.Win,
                    PieceInfos = board.GetPieceInfos()
                };
            }

            return new ChessMoveResult
            {
                Status = Status.Success,
                PieceInfos = board.GetPieceInfos()
            };
        }

        public int CreateAndSaveNewBoard(LobbyData lobbyData)
        {
            var board = new ChessBoard(true).SerializeContent();
            return _repository.InsertBoard(board);
        }

        public int CreateAndSaveDefaultBoard()
        {
            return CreateAndSaveNewBoard(null);
        }
    }
}
