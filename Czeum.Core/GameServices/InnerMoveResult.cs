using Czeum.Core.DTOs.Abstractions;

namespace Czeum.Core.GameServices
{
    /// <summary>
    /// A move result created by the IGameServices when executing a move.
    /// </summary>
    public class InnerMoveResult
    {
        public Status Status { get; set; }
        public IMoveResult MoveResult { get; set; }
    }
}