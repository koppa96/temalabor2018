using System;
using System.Threading.Tasks;
using Czeum.Core.DTOs.Abstractions;

namespace Czeum.Core.GameServices.MoveHandler
{
    public abstract class MoveHandler<TMoveData> : IMoveHandler
        where TMoveData : MoveData
    {
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
