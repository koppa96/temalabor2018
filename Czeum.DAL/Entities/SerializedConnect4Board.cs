using System;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.DTO.Connect4;

namespace Czeum.DAL.Entities
{
    public class SerializedConnect4Board : SerializedBoard
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public override MoveResult ToMoveResult()
        {
            var moveResult = new Connect4MoveResult
            {
                Status = Status.Requested,
                Board = new Item[Height, Width]
            };

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    switch (BoardData[i * Width + j])
                    {
                        case 'R':
                            moveResult.Board[i, j] = Item.Red;
                            break;
                        case 'Y':
                            moveResult.Board[i, j] = Item.Yellow;
                            break;
                        case 'N':
                            moveResult.Board[i, j] = Item.None;
                            break;
                        default:
                            throw new FormatException($"Error while processing the board data: {BoardData}");
                    }
                }
            }

            return moveResult;
        }
    }
}
