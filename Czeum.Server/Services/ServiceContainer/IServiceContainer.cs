using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;

namespace Czeum.Server.Services.ServiceContainer
{
    /// <summary>
    /// Interface for IGameService containers
    /// </summary>
    public interface IServiceContainer
    {
        /// <summary>
        /// Finds the game service that can execute the passed MoveData
        /// </summary>
        /// <param name="moveData">The move to be executed</param>
        /// <returns>The correct game service</returns>
        IGameService FindService(MoveData moveData);
        
        /// <summary>
        /// Finds the game service that can generate a default board for the passed LobbyData
        /// </summary>
        /// <param name="lobbyData">The lobby data</param>
        /// <returns>The correct game service</returns>
        IGameService FindService(LobbyData lobbyData);
        
        /// <summary>
        /// Gets a random game service from the contained services
        /// </summary>
        /// <returns>The random game service</returns>
        IGameService GetRandomService();
    }
}