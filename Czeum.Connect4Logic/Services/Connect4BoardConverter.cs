using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;
using Czeum.Abstractions.GameServices.BoardConverter;
using Czeum.Domain.Entities;
using Czeum.DTO.Connect4;
using System;
using System.Collections.Generic;
using System.Text;
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
