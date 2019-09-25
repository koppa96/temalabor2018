using System;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.Abstractions.GameServices;
using Czeum.DAL;
using Czeum.DAL.Entities;
using Czeum.DTO.Chess;

namespace Czeum.ChessLogic
{
    [GameService(typeof(ChessMoveData), typeof(ChessLobbyData), typeof(SerializedChessBoard))]
    public class ChessService : IGameService
    {
        public InnerMoveResult ExecuteMove(MoveData moveData, int playerId, ISerializedBoard serializedBoard)
        {
            var move = (ChessMoveData) moveData;
            var color = playerId == 1 ? Color.White : Color.Black;
            var enemyColor = playerId == 1 ? Color.Black : Color.White;
            var serializedChessBoard = (SerializedChessBoard) serializedBoard;

            var board = new ChessBoard(false);
            board.DeserializeContent(serializedChessBoard);

            if (!board.ValidateMove(move, color) || 
                !board.MovePiece(board[move.FromRow, move.FromColumn], board[move.ToRow, move.ToColumn]))
            {
                throw new InvalidOperationException("Invalid move.");
            }

            if (!board.IsKingSafe(color))
            {
                throw new InvalidOperationException("This move would put the king in check.");
            }

            var newBoardData = board.SerializeContent().BoardData;
            var enemyMoves = board.GetPossibleMovesFor(enemyColor);
            if (board.Stalemate(enemyColor, enemyMoves))
            {
                return new InnerMoveResult
                {
                    UpdatedBoardData = newBoardData,
                    Status = Status.Draw,
                    MoveResult = new ChessMoveResult
                    {
                        PieceInfos = board.GetPieceInfos(),
                        WhiteKingInCheck = !board.IsKingSafe(Color.White),
                        BlackKingInCheck = !board.IsKingSafe(Color.Black)
                    }
                };
            }

            if (board.Checkmate(enemyColor, enemyMoves))
            {
                return new InnerMoveResult
                {
                    UpdatedBoardData = newBoardData,
                    Status = Status.Win,
                    MoveResult = new ChessMoveResult
                    {
                        PieceInfos = board.GetPieceInfos(),
                        WhiteKingInCheck = !board.IsKingSafe(Color.White),
                        BlackKingInCheck = !board.IsKingSafe(Color.Black)
                    }
                };
            }

            return new InnerMoveResult
            {
                UpdatedBoardData = newBoardData,
                Status = Status.Success,
                MoveResult = new ChessMoveResult
                {
                    PieceInfos = board.GetPieceInfos(),
                    WhiteKingInCheck = !board.IsKingSafe(Color.White),
                    BlackKingInCheck = !board.IsKingSafe(Color.Black)
                }
            };
        }

        public ISerializedBoard CreateNewBoard(LobbyData lobbyData)
        {
            return new ChessBoard(true).SerializeContent();
        }

        public ISerializedBoard CreateDefaultBoard()
        {
            return CreateNewBoard(null);
        }

        public IMoveResult ConvertToMoveResult(ISerializedBoard serializedBoard)
        {
            var board = new ChessBoard(false);
            board.DeserializeContent((SerializedChessBoard) serializedBoard);

            return new ChessMoveResult
            {
                BlackKingInCheck = !board.IsKingSafe(Color.Black),
                WhiteKingInCheck = board.IsKingSafe(Color.White),
                PieceInfos = board.GetPieceInfos()
            };
        }
    }
}
