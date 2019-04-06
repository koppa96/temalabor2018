using Czeum.Abstractions.DTO;

namespace Czeum.Abstractions.GameServices
{
    public class InnerMoveResult
    {
        public string UpdatedBoardData { get; set; }
        public MoveResult MoveResult { get; set; }
    }
}