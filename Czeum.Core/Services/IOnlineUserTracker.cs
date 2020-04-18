using System.Collections.Generic;
using System.Threading.Tasks;

namespace Czeum.Core.Services
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
        /// <param name="connectionId"></param>
        void PutUser(string user, string connectionId);

        /// <summary>
        /// Removes a user from the online users.
        /// </summary>
        /// <param name="user">The name of the user</param>
        void RemoveUser(string user);

        /// <summary>
        /// Gets a list of the names of the currently online users.
        /// </summary>
        /// <returns>A list of online users</returns>
        IEnumerable<string> GetUsers();

        /// <summary>
        /// Determines whether the user with the given name is currently online.
        /// </summary>
        /// <param name="user">The name of the user</param>
        /// <returns>Whether the given user is online</returns>
        bool IsOnline(string user);

        /// <summary>
        /// Returns the user's connection id.
        /// </summary>
        /// <param name="user">The name of the user</param>
        /// <returns>The id of the connection</returns>
        string GetConnectionId(string user);

        /// <summary>
        /// Waits a predefined amount of time and returns whether the user has reconnected.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <returns>Whether the user has reconnected</returns>
        Task<bool> WaitTimeout(string username);
        
        /// <summary>
        /// Signals that the user has reconnected
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <returns>Whether the user has reconnected within the timeout</returns>
        bool OnReconnect(string username);
    }
}