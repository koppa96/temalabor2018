using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.Application.Models;
using Czeum.DTO;
using Czeum.DTO.Wrappers;

namespace Czeum.Application.Services.GameHandler
{
    /// <summary>
    /// Interface for services that interact with matches.
    /// </summary>
    public interface IGameHandler
    {
        /// <summary>
        /// Creates a match with a board from a LobbyData and persists them in the database.
        /// </summary>
        /// <param name="lobbyData">The lobby</param>
        /// <returns>A dictionary containing the match representations for each player</returns>
        Task<IEnumerable<MatchStatus>> CreateMatchAsync(LobbyData lobbyData);

        /// <summary>
        /// Creates a match with a random board and persists them in the database.
        /// </summary>
        /// <param name="players">A sequence containing the names of players that are participating</param>
        /// <returns>A dictionary containing the match representations for each player</returns>
        Task<IEnumerable<MatchStatus>> CreateRandomMatchAsync(IEnumerable<string> players);

        /// <summary>
        /// Dispatches the moves to the appropriate services that can execute them, and handles the execution results.
        /// </summary>
        /// <param name="moveData">The move</param>
        /// <returns>A dictionary containing the match representations for each player</returns>
        Task<IEnumerable<MatchStatus>> HandleMoveAsync(MoveData moveData);

        /// <summary>
        /// Gets a list of matches in which the player with the given name participates.
        /// </summary>
        /// <returns>The list of matches</returns>
        Task<IEnumerable<MatchStatus>> GetMatchesAsync();
    }
}