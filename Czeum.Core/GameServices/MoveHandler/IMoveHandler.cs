using System.Threading.Tasks;
using Czeum.Core.DTOs.Abstractions;

namespace Czeum.Core.GameServices.MoveHandler
{
    public interface IMoveHandler
    {
        Task<InnerMoveResult> HandleAsync(MoveData moveData, int playerId);
    }
}
