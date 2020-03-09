using System;
using System.Threading.Tasks;
using Czeum.Core.Domain;
using Czeum.Core.DTOs.Abstractions;

namespace Czeum.Core.GameServices.MoveHandler
{
    public abstract class MoveHandler<TMoveData, TSerializedBoard> : IMoveHandler
        where TMoveData : MoveData
        where TSerializedBoard : SerializedBoard
    {
        protected readonly IBoardLoader<TSerializedBoard> boardLoader;

        public MoveHandler(IBoardLoader<TSerializedBoard> boardLoader)
        {
            this.boardLoader = boardLoader;
        }

        public Task<InnerMoveResult> HandleAsync(MoveData moveData, int playerId)
        {
            if (!(moveData is TMoveData))
            {
                throw new NotSupportedException("This handler can not handler this type of move.");
            }

            return HandleAsync((TMoveData)moveData, playerId);
        }

        protected abstract Task<InnerMoveResult> HandleAsync(TMoveData moveData, int playerId);
    }
}
