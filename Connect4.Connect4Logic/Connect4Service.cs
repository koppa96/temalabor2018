using Connect4.Abstractions;
using Connect4.Entities;
using System;
using Czeum.DTO.Connect4;

namespace Connect4.Connect4Logic
{
    [GameService(ServiceNames.Connect4)]
    public class Connect4Service : IGameService
    {
        private readonly IBoardRepository<SerializedConnect4Board> _repository;

        public Connect4Service(IBoardRepository<SerializedConnect4Board> repository, IMatchRepository matchRepository)
        {
            _repository = repository;
        }

        public Status ExecuteMove(MoveData moveData, int playerId)
        {
            var move = moveData as Connect4MoveData;
            var board = new Connect4Board();
            var serializedBoard = _repository.GetByMatchId(move.MatchId);
            board.DeserializeContent(serializedBoard);
            
            var item = playerId == 1 ? Item.Red : Item.Yellow;
            if (!board.PlaceItem(item, move.Column))
            {
                return Status.Fail;
            }

            serializedBoard.BoardData = board.SerializeContent().BoardData;
            _repository.UpdateBoard(serializedBoard);

            if (board.CheckWinner() == item)
            {
                return Status.Win;
            }

            if (board.Full)
            {
                return Status.Draw;
            }

            return Status.Success;
        }
    }
}
