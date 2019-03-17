using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Abstractions
{
    public interface IMatchRepository
    {
        int GetPlayerIdByName(string name, int matchId);
    }
}
