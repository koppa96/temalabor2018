using System.Threading.Tasks;
using Czeum.Core.DTOs.Abstractions;

namespace Czeum.Core.GameServices.MoveHandler
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
