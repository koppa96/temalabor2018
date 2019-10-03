using System.Collections.Generic;

namespace Czeum.Application.Services.OnlineUsers
{
    /// <summary>
    /// Interface for online user tracking services.
    /// </summary>
    public interface IOnlineUserTracker
    {
        /// <summary>
        /// Adds a user to the online users.
        /// </summary>
        /// <param name="user">The name of the user</param>
        void PutUser(string user);

        /// <summary>
        /// Removes a user from the online users.
        /// </summary>
        /// <param name="user">The name of the user</param>
        void RemoveUser(string user);

        /// <summary>
        /// Gets a list of the names of the currently online users.
        /// </summary>
        /// <returns>A list of online users</returns>
        List<string> GetUsers();

        /// <summary>
        /// Determines whether the user with the given name is currently online.
        /// </summary>
        /// <param name="user">The name of the user</param>
        /// <returns>Whether the given user is online</returns>
        bool IsOnline(string user);
    }
}