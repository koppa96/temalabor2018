using Czeum.Core.DTOs.Abstractions;

namespace Czeum.Core.DTOs.Connect4
{
    public class Connect4MoveResult : IMoveResult
    {
        public Item[,] Board { get; set; }
    }
}
