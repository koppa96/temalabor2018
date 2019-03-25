using Czeum.Abstractions.DTO;

namespace Czeum.Abstractions.GameServices
{
    public interface IGameService
    {
        MoveResult ExecuteMove(MoveData moveData, int playerId);
        int CreateNewBoard(LobbyData lobbyData);
        int CreateDefaultBoard();
    }
}
