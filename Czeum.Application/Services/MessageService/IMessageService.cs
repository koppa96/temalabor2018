using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.DTO;

namespace Czeum.Application.Services.MessageService
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
        /// <param name="sender">The name of the sender</param>
        /// <returns>The sent message as a DTO if the sending was successful</returns>
        Message SendToLobby(int lobbyId, string message, string sender);

        /// <summary>
        /// Sends a message to a match.
        /// </summary>
        /// <param name="matchId">The identifier of the match</param>
        /// <param name="message">The content of the message</param>
        /// <param name="sender">The name of the sender</param>
        /// <returns>The sent message as a DTO if the sending was successful</returns>
        Task<Message> SendToMatchAsync(Guid matchId, string message, string sender);

        /// <summary>
        /// Gets all the messages that were sent to a specific lobby.
        /// </summary>
        /// <param name="lobbyId">The identifier of the lobby</param>
        /// <returns>The list of messages</returns>
        List<Message> GetMessagesOfLobby(int lobbyId);

        /// <summary>
        /// Gets all the messages that were sent to a specific match.
        /// </summary>
        /// <param name="matchId">The identifier of the match</param>
        /// <returns>The list of messages</returns>
        Task<List<Message>> GetMessagesOfMatchAsync(Guid matchId);
    }
}