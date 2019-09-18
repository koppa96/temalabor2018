using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.DAL.Entities;
using Czeum.DTO;

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
        Task<Dictionary<string, MatchStatus>> CreateMatchAsync(LobbyData lobbyData);

        /// <summary>
        /// Creates a match with a random board and persists them in the database.
        /// </summary>
        /// <param name="player1">The name of the first player</param>
        /// <param name="player2">The name of the second player</param>
        /// <returns>A dictionary containing the match representations for each player</returns>
        Task<Dictionary<string, MatchStatus>> CreateRandomMatchAsync(string player1, string player2);

        /// <summary>
        /// Dispatches the moves to the appropriate services that can execute them, and handles the execution results.
        /// </summary>
        /// <param name="moveData">The move</param>
        /// <param name="playerId">The id of the player in the match</param>
        /// <returns>A dictionary containing the match representations for each player</returns>
        Task<Dictionary<string, MatchStatus>> HandleMoveAsync(MoveData moveData, int playerId);

        /// <summary>
        /// Gets a match by its match identifier asynchronously
        /// </summary>
        /// <param name="id">The identifier of the match</param>
        /// <returns>The match</returns>
        Task<Match> GetMatchByIdAsync(int id);

        /// <summary>
        /// Gets a list of matches in which the player with the given name participates.
        /// </summary>
        /// <param name="player">The name of the player</param>
        /// <returns>The list of matches</returns>
        Task<List<MatchStatus>> GetMatchesOfPlayerAsync(string player);

        /// <summary>
        /// Gets a board by the identifier of the match associated with the board.
        /// </summary>
        /// <param name="matchId">The identifier of the match</param>
        /// <returns>The board as a MoveResult</returns>
        Task<MoveResult> GetBoardByMatchIdAsync(int matchId);
    }
}