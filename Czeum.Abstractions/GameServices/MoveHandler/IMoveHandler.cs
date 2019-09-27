using Czeum.Abstractions.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Abstractions.GameServices.MoveHandler
{
    public interface IMoveHandler
    {
        Task<InnerMoveResult> HandleAsync(MoveData moveData, int playerId);
    }
}
