using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;
using Czeum.DAL.Entities;
using Czeum.DAL.Interfaces;
using Czeum.DTO.Connect4;

namespace Czeum.Connect4Logic
{
    [GameService(typeof(Connect4MoveData), typeof(Connect4LobbyData))]
    public class Connect4Service : IGameService
    {
        public InnerMoveResult ExecuteMove(MoveData moveData, int playerId, ISerializedBoard serializedBoard)
        {
            var move = (Connect4MoveData) moveData;
            var board = new Connect4Board();
            board.DeserializeContent((SerializedConnect4Board) serializedBoard);
            
            var item = playerId == 1 ? Item.Red : Item.Yellow;

            if (!board.PlaceItem(item, move.Column))
            {
                return new InnerMoveResult
                {
                    UpdatedBoardData = serializedBoard.BoardData,
                    MoveResult = new Connect4MoveResult
                    {
                        Status = Status.Fail,
                        Board = board.Board
                    }
                };
            }

            var newBoardData = board.SerializeContent().BoardData;
            var result = new InnerMoveResult
            {
                UpdatedBoardData = newBoardData,
                MoveResult = new Connect4MoveResult
                {
                    Board = board.Board
                }
            };
            
            if (board.CheckWinner() == item)
            {
                result.MoveResult.Status = Status.Win;
                return result;
            }

            if (board.Full)
            {
                result.MoveResult.Status = Status.Draw;
                return result;
            }

            result.MoveResult.Status = Status.Success;
            return result;
        }

        public ISerializedBoard CreateNewBoard(LobbyData lobbyData)
        {
            var lobby = (Connect4LobbyData) lobbyData;
            return new Connect4Board(lobby.BoardWidth, lobby.BoardHeight).SerializeContent();
        }

        public ISerializedBoard CreateDefaultBoard()
        {
            var lobby = new Connect4LobbyData();
            return CreateNewBoard(lobby);
        }
    }
}
