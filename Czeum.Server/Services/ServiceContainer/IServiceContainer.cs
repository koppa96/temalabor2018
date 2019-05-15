using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;
using Czeum.DAL.Entities;
using Czeum.Abstractions;

namespace Czeum.Server.Services.ServiceContainer
{
    /// <summary>
    /// Interface for IGameService containers
    /// </summary>
    public interface IServiceContainer
    {
        /// <summary>
        /// Finds the game service that can execute the passed MoveData.
        /// </summary>
        /// <param name="moveData">The move to be executed</param>
        /// <exception cref="GameNotSupportedException">Thrown when there is no suitable service for the move</exception>
        /// <returns>The correct game service</returns>
        IGameService FindByMoveData(MoveData moveData);
        
        /// <summary>
        /// Finds the game service that can generate a default board for the passed LobbyData.
        /// </summary>
        /// <param name="lobbyData">The lobby data</param>
        /// <exception cref="GameNotSupportedException">Thrown when there is no suitable service for the lobby</exception>
        /// <returns>The correct game service</returns>
        IGameService FindByLobbyData(LobbyData lobbyData);

        /// <summary>
        /// Finds the game service that can transform the serialized board into a MoveResult.
        /// </summary>
        /// <param name="serializedBoard">The serialized board</param>
        /// <exception cref="GameNotSupportedException">Thrown when there is no suitable service for the board</exception>
        /// <returns>The correct game service</returns>
        IGameService FindBySerializedBoard(SerializedBoard serializedBoard);
        
        /// <summary>
        /// Gets a random game service from the contained services.
        /// </summary>
        /// <returns>The random game service</returns>
        IGameService GetRandomService();
    }
}