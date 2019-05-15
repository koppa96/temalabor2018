using Czeum.Abstractions.DTO;

namespace Czeum.Abstractions.GameServices
{
    /// <summary>
    /// A move result created by the IGameServices when executing a move.
    /// </summary>
    public class InnerMoveResult
    {
        public string UpdatedBoardData { get; set; }
        public MoveResult MoveResult { get; set; }
    }
}