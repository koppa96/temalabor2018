using Connect4.Abstractions;
using Connect4.Entities;
using System;

namespace Connect4.Connect4Logic
{
    public class Connect4Service : IGameService
    {
        private readonly IBoardRepository<SerializedConnect4Board> _repository;

        public Connect4Service(IBoardRepository<SerializedConnect4Board> repository)
        {
            _repository = repository;
        }

        public MoveResult ExecuteMove(MoveData moveData)
        {
            var move = moveData as Connect4MoveData;
            var board = new Connect4Board();
            var serializedBoard = _repository.GetByMatchId(move.MatchId);
            board.FillFromSerialized(serializedBoard);

            if (!board.PlaceItem(move.Item, move.Column))
            {
                throw new ArgumentException("The selected column is full.");
            }

            serializedBoard.BoardData = board.ToSerialized().BoardData;
            _repository.UpdateBoard(serializedBoard);

            if (board.CheckWinner() == move.Item)
            {
                return MoveResult.Win;
            }

            if (board.Full)
            {
                return MoveResult.Draw;
            }

            return MoveResult.Success;
        }
    }
}
