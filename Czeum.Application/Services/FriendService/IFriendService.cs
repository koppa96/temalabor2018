using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.DTO.UserManagement;

namespace Czeum.Application.Services.FriendService
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
        /// <param name="requestId">The identifier of the friend request</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task<(FriendDto Sender, FriendDto Receiver)> AcceptRequestAsync(Guid requestId);

        /// <summary>
        /// Removes a friend from the friends of the user.
        /// </summary>
        /// <param name="friendshipId">The identifier of the friendship</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task RemoveFriendAsync(Guid friendshipId);

        /// <summary>
        /// Adds a friend request.
        /// </summary>
        /// <param name="receiver">The receiver of the request</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task<FriendRequestDto> AddRequestAsync(string receiver);

        /// <summary>
        /// Removes a friend request sent by the sender and received by the receiver.
        /// </summary>
        /// <param name="requestId">The identifier of the request</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task RemoveRequestAsync(Guid requestId);

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
    }
}