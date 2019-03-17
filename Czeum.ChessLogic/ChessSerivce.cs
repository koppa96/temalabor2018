using Czeum.Abstractions;
using Czeum.Entities;
using System;

namespace Czeum.ChessLogic
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
