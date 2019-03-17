using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Abstractions
{
    public interface IGameService
    {
        Status ExecuteMove(MoveData move, int playerId);
    }
}
