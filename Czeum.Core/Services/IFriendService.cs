using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Core.DTOs.UserManagement;

namespace Czeum.Core.Services
{
    /// <summary>
    /// Interface for services related to friend handling.
    /// </summary>
    public interface IFriendService
    {
        /// <summa>
        /// Gets the names of the user's friends.
        /// </summary>
        /// <param name="user">The name of the user</param>
        /// <returns>The names of the friends</returns>
        Task<IEnumerable<FriendDto>> GetFriendsOfUserAsync(string user);

        /// <summary>
        /// Accepts the request sent by the sender and received by the receiver.
        /// </summary>
        /// <param name="requestId">The identifier of the friend request</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task<FriendDto> AcceptRequestAsync(Guid requestId);

        /// <summary>
        /// Removes a friend from the friends of the user.
        /// </summary>
        /// <param name="friendshipId">The identifier of the friendship</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task RemoveFriendAsync(Guid friendshipId);

        /// <summary>
        /// Adds a friend request.
        /// </summary>
        /// <param name="receiverId">The receiver of the request</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task<FriendRequestDto> AddRequestAsync(Guid receiverId);

        /// <summary>
        /// Rejects the friend request
        /// </summary>
        /// <param name="requestId">The identifier of the request</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task RejectRequestAsync(Guid requestId);

        /// <summary>
        /// Revokes the friend request
        /// </summary>
        /// <param name="requestId">The identifier of the request</param>
        /// <returns></returns>
        Task RevokeRequestAsync(Guid requestId);

        /// <summary>
        /// Gets the names of the users to whom the given user has sent friend request.
        /// </summary>
        /// <param name="user">The name of the user</param>
        /// <returns>The list of usernames</returns>
        Task<IEnumerable<FriendRequestDto>> GetRequestsSentAsync();

        /// <summary>
        /// Gets the names of the users from whom the given user has received friend request.
        /// </summary>
        /// <param name="user">The name of the user</param>
        /// <returns>The list of usernames</returns>
        Task<IEnumerable<FriendRequestDto>> GetRequestsReceivedAsync();

        /// <summary>
        /// Gets the up to date friend data for notifications, with the names of the friends to be notified.
        /// </summary>
        /// <param name="username">The name of the player</param>
        /// <returns>The notification data with the names of the users to be notified</returns>
        Task<Dictionary<string, FriendDto>> GetNotificationDataAsync(string username);
    }
}