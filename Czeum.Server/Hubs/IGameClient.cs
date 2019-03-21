using System.Threading.Tasks;
using Czeum.DTO;

namespace Czeum.Server.Hubs
{
    public interface IGameClient
    {
        Task NotYourMatch();
        Task ReceiveResult(MatchStatus status);
        Task NotYourTurn();
    }
}