using Czeum.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Abstractions.GameServices
{
    public interface IBoardLoader<TBoard>
        where TBoard : ISerializedBoard
    {
        Task<TBoard> LoadByMatchIdAsync(Guid matchId);
    }
}
