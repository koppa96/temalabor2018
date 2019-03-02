using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4.Abstractions
{
    public interface IGameService
    {
        MoveResult ExecuteMove(MoveData move);
    }
}
