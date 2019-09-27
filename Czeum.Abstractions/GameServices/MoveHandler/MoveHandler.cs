using Czeum.Abstractions.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Abstractions.GameServices.MoveHandler
{
    public abstract class MoveHandler<TMoveData> : IMoveHandler
        where TMoveData : MoveData
    {
        public Task<InnerMoveResult> HandleAsync(MoveData moveData, int playerId)
        {
            return HandleAsync((TMoveData)moveData, playerId);
        }

        protected abstract Task<InnerMoveResult> HandleAsync(TMoveData moveData, int playerId);
    }
}
