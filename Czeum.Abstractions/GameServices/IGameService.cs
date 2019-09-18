using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;

namespace Czeum.Abstractions.GameServices
{
    /// <summary>
    /// An abstract service that handles the operations with a specific game type.
    /// </summary>
    public interface IGameService
    {
        /// <summary>
        /// Executes the MoveData related to that game.
        /// </summary>
        /// <param name="moveData">The move to be executed</param>
        /// <param name="playerId">The identifier of the player</param>
        /// <param name="board">The board</param>
        /// <returns></returns>
        InnerMoveResult ExecuteMove(MoveData moveData, int playerId, ISerializedBoard board);

        /// <summary>
        /// Creates a board with the settings in the given lobby.
        /// </summary>
        /// <param name="lobbyData">The lobby</param>
        /// <returns>The serialized representation of the board</returns>
        ISerializedBoard CreateNewBoard(LobbyData lobbyData);

        /// <summary>
        /// Creates a board with the default settings.
        /// </summary>
        /// <returns>The serialized representation of the board</returns>
        ISerializedBoard CreateDefaultBoard();

        /// <summary>
        /// Converts a serialized board into a MoveResult that can be sent to the clients.
        /// </summary>
        /// <param name="serializedBoard">The serialized board</param>
        /// <returns>The MoveResult representation of the board</returns>
        MoveResult ConvertToMoveResult(ISerializedBoard serializedBoard);
    }
}
