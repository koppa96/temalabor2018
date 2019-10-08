using Czeum.Core.DTOs.Abstractions;
using Czeum.Core.DTOs.Connect4;
using Czeum.Core.GameServices.BoardConverter;
using Czeum.Domain.Entities.Boards;

namespace Czeum.Connect4Logic.Services
{
    public class Connect4BoardConverter : BoardConverter<SerializedConnect4Board>
    {
        public override IMoveResult Convert(SerializedConnect4Board serializedBoard)
        {
            var board = new Connect4Board();
            board.DeserializeContent(serializedBoard);

            return new Connect4MoveResult
            {
                Board = board.Board
            };
        }
    }
}
