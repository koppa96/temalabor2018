using System.Threading.Tasks;
using Czeum.DTO;

namespace Czeum.ClientCallback
{
    public interface IGameClient
    {
        Task ReceiveResult(MatchStatus status);
        Task MatchCreated(MatchStatus status);
        Task MatchMessageSent(int matchId, Message message);
        Task ReceiveMatchMessage(int matchId, Message message);
    }
}