using System;
using System.Threading.Tasks;
using Czeum.Core.Domain;

namespace Czeum.Core.GameServices
{
    public interface IBoardLoader<TBoard>
        where TBoard : SerializedBoard
    {
        Task<TBoard> LoadByMatchIdAsync(Guid matchId);
    }
}
