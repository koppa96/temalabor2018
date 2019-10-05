using System;
using System.Threading.Tasks;
using Czeum.DTO;

namespace Czeum.ClientCallback
{
    /// <summary>
    /// An interface that the GameHub uses to notify its clients about Match events.
    /// </summary>
    public interface IGameClient
    {
        Task ReceiveResult(MatchStatus status);
        Task MatchCreated(MatchStatus status);
        Task ReceiveMatchMessage(Guid matchId, Message message);
    }
}