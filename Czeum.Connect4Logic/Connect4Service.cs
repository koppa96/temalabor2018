using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;
using Czeum.DAL.Entities;
using Czeum.DAL.Interfaces;
using Czeum.DTO.Connect4;

namespace Czeum.Connect4Logic
{
    [GameService(ServiceFinder.Connect4)]
    public class Connect4Service : IGameService
    {
        private readonly IBoardRepository<SerializedConnect4Board> _repository;

        public Connect4Service(IBoardRepository<SerializedConnect4Board> repository)
        {
            _repository = repository;
        }

        public MoveResult ExecuteMove(MoveData moveData, int playerId)
        {
            var move = (Connect4MoveData) moveData;
            var board = new Connect4Board();
            var serializedBoard = _repository.GetByMatchId(move.MatchId);
            board.DeserializeContent(serializedBoard);
            
            var item = playerId == 1 ? Item.Red : Item.Yellow;

            if (!board.PlaceItem(item, move.Column))
            {
                return new Connect4MoveResult
                {
                    Status = Status.Fail,
                    Board = board.Board
                };
            }

            _repository.UpdateBoardData(serializedBoard.BoardId, board.SerializeContent().BoardData);
            var result = new Connect4MoveResult
            {
                Board = board.Board
            };
            if (board.CheckWinner() == item)
            {
                result.Status = Status.Win;
                return result;
            }

            if (board.Full)
            {
                result.Status = Status.Draw;
                return result;
            }

            result.Status = Status.Success;
            return result;
        }

        public int CreateNewBoard(LobbyData lobbyData)
        {
            var lobby = (Connect4LobbyData) lobbyData;
            var board = new Connect4Board(lobby.BoardWidth, lobby.BoardHeight).SerializeContent();
            return _repository.InsertBoard(board);
        }

        public int CreateDefaultBoard()
        {
            var lobby = new Connect4LobbyData();
            return CreateNewBoard(lobby);
        }
    }
}
