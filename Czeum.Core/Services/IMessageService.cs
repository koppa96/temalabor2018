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
        /// Sends a message to a friendship.
        /// </summary>
        /// <param name="friendshipId">The identifier of the friendship</param>
        /// <param name="message">The content of the message</param>
        /// <returns></returns>
        Task<Message> SendToFriendAsync(Guid friendshipId, string message);

        /// <summary>
        /// Gets the requested amount of the latest messages sent before the message with the given id.
        /// </summary>
        /// <param name="lobbyId">The identifier of the lobby</param>
        /// <param name="oldestId">The identifier of the message before the newest message</param>
        /// <param name="requestedCount">The amount of messages requested</param>
        /// <returns>The list of messages</returns>
        Task<RollListDto<Message>> GetMessagesOfLobbyAsync(Guid lobbyId, Guid? oldestId, int requestedCount);

        /// <summary>
        /// Gets the requested amount of the latest messages sent before the message with the given id.
        /// </summary>
        /// <param name="matchId">The identifier of the match</param>
        /// <param name="oldestId">The identifier of the message before the newest message</param>
        /// <param name="requestedCount">The amount of messages requested</param>
        /// <returns>The list of messages</returns>
        Task<RollListDto<Message>> GetMessagesOfMatchAsync(Guid matchId, Guid? oldestId, int requestedCount);

        /// <summary>
        /// Gets the requested amount of the latest messages sent before the message with the given id.
        /// </summary>
        /// <param name="friendshipId">The identifier of the friendship</param>
        /// <param name="oldestId">The identifier of the message before the newest message</param>
        /// <param name="requestedCount">The amount of messages requested</param>
        /// <returns></returns>
        Task<RollListDto<Message>> GetMessagesOfFrienshipAsync(Guid friendshipId, Guid? oldestId, int requestedCount);
    }
}