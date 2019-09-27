using Czeum.Abstractions.DTO;

namespace Czeum.Abstractions.GameServices
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