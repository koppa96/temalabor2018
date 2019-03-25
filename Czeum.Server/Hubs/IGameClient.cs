using System.Threading.Tasks;
using Czeum.DTO;

namespace Czeum.Server.Hubs
{
    public interface IGameClient
    {
        Task ReceiveResult(MatchStatus status);
        Task MatchCreated(MatchStatus status);
    }
}