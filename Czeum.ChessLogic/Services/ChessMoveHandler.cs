using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;
using Czeum.Abstractions.GameServices.MoveHandler;
using Czeum.DAL;
using Czeum.DAL.Extensions;
using Czeum.Domain.Entities;
using Czeum.DTO.Chess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.ChessLogic.Services
{
    public class ChessMoveHandler : MoveHandler<ChessMoveData>
    {
        private readonly IBoardLoader<SerializedChessBoard> boardLoader;

        public ChessMoveHandler(IBoardLoader<SerializedChessBoard> boardLoader)
        {
            this.boardLoader = boardLoader;
        }

        protected override async Task<InnerMoveResult> HandleAsync(ChessMoveData moveData, int playerId)
        {
            var serializedChessBoard = await boardLoader.LoadByMatchIdAsync(moveData.MatchId);

            var board = new ChessBoard(false);
            board.DeserializeContent(serializedChessBoard);

            var color = playerId == 1 ? Color.White : Color.Black;
            var enemyColor = playerId == 1 ? Color.Black : Color.White;

            if (!board.ValidateMove(moveData, color) ||
                !board.MovePiece(board[moveData.FromRow, moveData.FromColumn], board[moveData.ToRow, moveData.ToColumn]))
            {
                throw new InvalidOperationException("Invalid move.");
            }

            if (!board.IsKingSafe(color))
            {
                throw new InvalidOperationException("This move would put the king in check.");
            }

            serializedChessBoard.BoardData = board.SerializeContent().BoardData;
            var enemyMoves = board.GetPossibleMovesFor(enemyColor);
            if (board.Stalemate(enemyColor, enemyMoves))
            {
                return new InnerMoveResult
                {
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
                Status = Status.Success,
                MoveResult = new ChessMoveResult
                {
                    PieceInfos = board.GetPieceInfos(),
                    WhiteKingInCheck = !board.IsKingSafe(Color.White),
                    BlackKingInCheck = !board.IsKingSafe(Color.Black)
                }
            };
        }
    }
}
