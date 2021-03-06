using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Abstractions;
using Czeum.Core.DTOs.Paging;

namespace Czeum.Core.Services
{
    /// <summary>
    /// Interface for services that interact with matches.
    /// </summary>
    public interface IMatchService
    {
        /// <summary>
        /// Creates a match with a board from a LobbyData and persists them in the database.
        /// </summary>
        /// <param name="lobbyId"></param>
        /// <returns>A dictionary containing the match representations for each player</returns>
        Task<MatchStatus> CreateMatchAsync(Guid lobbyId);

        /// <summary>
        /// Creates a match with a random board and persists them in the database.
        /// </summary>
        /// <param name="players">A sequence containing the names of players that are participating</param>
        /// <returns>A dictionary containing the match representations for each player</returns>
        Task CreateRandomMatchAsync(IEnumerable<string> players);

        /// <summary>
        /// Dispatches the moves to the appropriate services that can execute them, and handles the execution results.
        /// </summary>
        /// <param name="moveData">The move</param>
        /// <returns>A dictionary containing the match representations for each player</returns>
        Task<MatchStatus> HandleMoveAsync(MoveData moveData);

        /// <summary>
        /// Gets a list of matches in which the player with the given name participates.
        /// </summary>
        /// <returns>The list of matches</returns>
        Task<IEnumerable<MatchStatus>> GetMatchesAsync();

        Task<RollListDto<MatchStatus>> GetCurrentMatchesAsync(Guid? oldestId, int count);

        Task<RollListDto<MatchStatus>> GetFinishedMatchesAsync(Guid? oldestId, int count);

        /// <summary>
        /// Returns a list with the names of the other players that are playing in the match.
        /// </summary>
        /// <param name="matchId">The id of the match</param>
        /// <returns>A list of player names</returns>
        Task<IEnumerable<string>> GetOthersInMatchAsync(Guid matchId);
        Task<IEnumerable<GameTypeDto>> GetAvailableGameTypesAsync();
        Task<MatchStatus> GetMatchAsync(Guid matchId);
        Task<MatchStatus> CallDrawAsync(Guid matchId);
        Task<MatchStatus> ResignAsync(Guid matchId);
    }
}