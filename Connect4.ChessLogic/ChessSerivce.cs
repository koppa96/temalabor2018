using Connect4.Abstractions;
using Connect4.Entities;
using System;

namespace Connect4.ChessLogic
{
    [GameService(ServiceNames.Chess)]
    public class ChessService : IGameService
    {
        private readonly IBoardRepository<SerializedChessBoard> _repository;

        public ChessService(IBoardRepository<SerializedChessBoard> repository)
        {
            _repository = repository;
        }

        public Status ExecuteMove(MoveData move, int playerId)
        {
            throw new NotImplementedException();
        }
    }
}
