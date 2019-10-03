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
        Task<MatchStatusResult> CreateMatchAsync(LobbyData lobbyData);

        /// <summary>
        /// Creates a match with a random board and persists them in the database.
        /// </summary>
        /// <param name="player1">The name of the first player</param>
        /// <param name="player2">The name of the second player</param>
        /// <returns>A dictionary containing the match representations for each player</returns>
        Task<MatchStatusResult> CreateRandomMatchAsync(string player1, string player2);

        /// <summary>
        /// Dispatches the moves to the appropriate services that can execute them, and handles the execution results.
        /// </summary>
        /// <param name="moveData">The move</param>
        /// <param name="username">The name of the player</param>
        /// <returns>A dictionary containing the match representations for each player</returns>
        Task<MatchStatusResult> HandleMoveAsync(MoveData moveData);

        /// <summary>
        /// Gets a list of matches in which the player with the given name participates.
        /// </summary>
        /// <param name="player">The name of the player</param>
        /// <returns>The list of matches</returns>
        Task<IEnumerable<MatchStatus>> GetMatchesOfPlayerAsync(string player);
    }
}