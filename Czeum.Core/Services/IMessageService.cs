using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Paging;

namespace Czeum.Core.Services
{
    /// <summary>
    /// Interface for in-game messaging services.
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Sends a message to a lobby.
        /// </summary>
        /// <param name="lobbyId">The identifier of the lobby</param>
        /// <param name="message">The content of the message</param>
        /// <returns>The sent message as a DTO if the sending was successful</returns>
        Task<Message> SendToLobbyAsync(Guid lobbyId, string message);

        /// <summary>
        /// Sends a message to a match.
        /// </summary>
        /// <param name="matchId">The identifier of the match</param>
        /// <param name="message">The content of the message</param>
        /// <returns>The sent message as a DTO if the sending was successful</returns>
        Task<Message> SendToMatchAsync(Guid matchId, string message);

        /// <summary>
        /// Gets all the messages that were sent to a specific lobby.
        /// </summary>
        /// <param name="lobbyId">The identifier of the lobby</param>
        /// <param name="oldestId"></param>
        /// <param name="requestedCount"></param>
        /// <returns>The list of messages</returns>
        Task<RollListDto<Message>> GetMessagesOfLobbyAsync(Guid lobbyId, Guid? oldestId, int requestedCount);

        /// <summary>
        /// Gets all the messages that were sent to a specific match.
        /// </summary>
        /// <param name="matchId">The identifier of the match</param>
        /// <param name="oldestId"></param>
        /// <param name="requestedCount"></param>
        /// <returns>The list of messages</returns>
        Task<RollListDto<Message>> GetMessagesOfMatchAsync(Guid matchId, Guid? oldestId, int requestedCount);
    }
}