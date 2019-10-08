using Czeum.Core.Domain;
using Czeum.Core.DTOs.Connect4;
using Czeum.Core.GameServices.BoardCreator;

namespace Czeum.Connect4Logic.Services
{
    public class Connect4BoardCreator : BoardCreator<Connect4LobbyData>
    {
        public override SerializedBoard CreateBoard(Connect4LobbyData lobbyData)
        {
            return new Connect4Board(lobbyData.BoardWidth, lobbyData.BoardHeight).SerializeContent();
        }

        public override SerializedBoard CreateDefaultBoard()
        {
            var lobby = new Connect4LobbyData();
            return CreateBoard(lobby);
        }
    }
}
