using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4.Abstractions
{
    public interface IMatchRepository
    {
        int GetPlayerIdByName(string name, int matchId);
    }
}
