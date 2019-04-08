using System.Collections.Generic;

namespace Czeum.Server.Services.FriendService
{
    public interface IFriendService
    {
        List<string> GetFriendsOf(string user);
        void AcceptRequest(string sender, string receiver);
        void RemoveFriend(string user, string friend);
        void AddRequest(string sender, string receiver);
        void RemoveRequest(string sender, string receiver);
        List<string> GetRequestsSentBy(string user);
        List<string> GetRequestsReceivedBy(string user);
    }
}