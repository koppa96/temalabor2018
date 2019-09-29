using System;
using System.Collections.Generic;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.Application.Models;
using Czeum.DAL.Exceptions;
using Czeum.DTO;

namespace Czeum.Application.Services.Lobby
{
    /// <summary>
    /// Interface for lobby storing services.
    /// </summary>
    public interface ILobbyStorage
    {
        /// <summary>
        /// Gets all the lobbies that are stored.
        /// </summary>
        /// <returns>The lobbies</returns>
        IEnumerable<LobbyData> GetLobbies();

        /// <summary>
        /// Gets a specific lobby with the given identifier.
        /// </summary>
        /// <param name="lobbyId">The identifier of the lobby</param>
        /// <exception cref="NotFoundException">Thrown when there is no lobby with the given id.</exception>
        /// <returns>The lobby</returns>
        LobbyData GetLobby(Guid lobbyId);

        /// <summary>
        /// Adds a lobby to the storage.
        /// </summary>
        /// <param name="lobbyData">The lobby to be added</param>
        void AddLobby(LobbyData lobbyData);

        /// <summary>
        /// Removes the lobby with the given ID from the storage.
        /// </summary>
        /// <param name="lobbyId">The identifier of the lobby</param>
        void RemoveLobby(Guid lobbyId);

        /// <summary>
        /// Updates the given lobby's settings.
        /// </summary>
        /// <param name="lobbyData">The updated lobby</param>
        void UpdateLobby(LobbyData lobbyData);

        /// <summary>
        /// Gets the lobby to which the user is connected.
        /// </summary>
        /// <param name="user">The name of the user</param>
        /// <returns>The lobby of the user</returns>
        LobbyData? GetLobbyOfUser(string user);

        /// <summary>
        /// Adds a message to a lobby.
        /// </summary>
        /// <param name="lobbyId">The identifier of the lobby</param>
        /// <param name="message">The message to be added</param>
        void AddMessage(Guid lobbyId, Message message);

        /// <summary>
        /// Gets the messages associated with the given lobby.
        /// </summary>
        /// <param name="lobbyId">The identifier of the lobby</param>
        /// <returns>The list of messages</returns>
        List<Message> GetMessages(Guid lobbyId);

        bool LobbyExitsts(Guid lobbyId);
    }
}