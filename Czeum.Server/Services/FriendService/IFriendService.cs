using System.Collections.Generic;
using System.Threading.Tasks;

namespace Czeum.Server.Services.FriendService
{
    /// <summary>
    /// Interface for services related to friend handling.
    /// </summary>
    public interface IFriendService
    {
        /// <summary>
        /// Gets the names of the user's friends.
        /// </summary>
        /// <param name="user">The name of the user</param>
        /// <returns>The names of the friends</returns>
        Task<List<string>> GetFriendsOfUserAsync(string user);

        /// <summary>
        /// Accepts the request sent by the sender and received by the receiver.
        /// </summary>
        /// <param name="sender">The sender of the request</param>
        /// <param name="receiver">The receiver of the request</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task AcceptRequestAsync(string sender, string receiver);

        /// <summary>
        /// Removes a friend from the friends of the user.
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="friend">The friend of the user</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task RemoveFriendAsync(string user, string friend);

        /// <summary>
        /// Adds a friend request.
        /// </summary>
        /// <param name="sender">The sender of the request</param>
        /// <param name="receiver">The receiver of the request</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task AddRequestAsync(string sender, string receiver);

        /// <summary>
        /// Removes a friend request sent by the sender and received by the receiver.
        /// </summary>
        /// <param name="sender">The sender of the request</param>
        /// <param name="receiver">The receiver of the request</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task RemoveRequestAsync(string sender, string receiver);

        /// <summary>
        /// Gets the names of the users to whom the given user has sent friend request.
        /// </summary>
        /// <param name="user">The name of the user</param>
        /// <returns>The list of usernames</returns>
        Task<List<string>> GetRequestsSentByUserAsync(string user);

        /// <summary>
        /// Gets the names of the users from whom the given user has received friend request.
        /// </summary>
        /// <param name="user">The name of the user</param>
        /// <returns>The list of usernames</returns>
        Task<List<string>> GetRequestsReceivedByUserAsync(string user);
    }
}