using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;
using Czeum.Abstractions.GameServices.MoveHandler;
using Czeum.Domain.Entities;
using Czeum.DTO.Connect4;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Czeum.Domain.Entities.Boards;

namespace Czeum.Connect4Logic.Services
{
    public class Connect4MoveHandler : MoveHandler<Connect4MoveData>
    {
        private readonly IBoardLoader<SerializedConnect4Board> boardLoader;

        public Connect4MoveHandler(IBoardLoader<SerializedConnect4Board> boardLoader)
        {
            this.boardLoader = boardLoader;
        }

        protected override async Task<InnerMoveResult> HandleAsync(Connect4MoveData moveData, int playerId)
        {
            var serializedBoard = await boardLoader.LoadByMatchIdAsync(moveData.MatchId);

            var board = new Connect4Board();
            board.DeserializeContent(serializedBoard);

            var item = playerId == 1 ? Item.Red : Item.Yellow;

            board.PlaceItem(item, moveData.Column);
            serializedBoard.BoardData = board.SerializeContent().BoardData;
            var result = new InnerMoveResult
            {
                MoveResult = new Connect4MoveResult
                {
                    Board = board.Board
                }
            };

            if (board.CheckWinner() == item)
            {
                result.Status = Status.Win;
                return result;
            }

            if (board.Full)
            {
                result.Status = Status.Draw;
                return result;
            }

            result.Status = Status.Success;
            return result;
        }
    }
}
