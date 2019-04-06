using Czeum.Abstractions.DTO;

namespace Czeum.Abstractions
{
    public interface ISerializedBoard
    {
        string BoardData { get; set; }
        
        MoveResult ToMoveResult();
    }
}