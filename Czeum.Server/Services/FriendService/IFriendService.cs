using System.Collections.Generic;
using System.Threading.Tasks;

namespace Czeum.Server.Services.FriendService
{
    public interface IFriendService
    {
        Task<List<string>> GetFriendsOfUserAsync(string user);
        Task AcceptRequestAsync(string sender, string receiver);
        Task RemoveFriendAsync(string user, string friend);
        Task AddRequestAsync(string sender, string receiver);
        Task RemoveRequestAsync(string sender, string receiver);
        Task<List<string>> GetRequestsSentByUserAsync(string user);
        Task<List<string>> GetRequestsReceivedByUserAsync(string user);
    }
}