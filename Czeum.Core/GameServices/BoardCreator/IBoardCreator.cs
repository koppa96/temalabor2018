using Czeum.Core.Domain;
using Czeum.Core.DTOs.Abstractions.Lobbies;

namespace Czeum.Core.GameServices.BoardCreator
{
    public interface IBoardCreator
    {
        SerializedBoard CreateBoard(LobbyData lobbyData);
        SerializedBoard CreateDefaultBoard();
    }
}
