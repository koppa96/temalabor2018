using Czeum.Abstractions.DTO;

namespace Czeum.Abstractions.GameServices
{
    public interface IGameService
    {
        InnerMoveResult ExecuteMove(MoveData moveData, int playerId, ISerializedBoard board);
        ISerializedBoard CreateNewBoard(LobbyData lobbyData);
        ISerializedBoard CreateDefaultBoard();
    }
}
