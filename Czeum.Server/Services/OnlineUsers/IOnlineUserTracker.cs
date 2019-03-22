using System.Collections.Generic;

namespace Czeum.Server.Services.OnlineUsers
{
    public interface IOnlineUserTracker
    {
        void PutUser(string user);
        void RemoveUser(string user);
        List<string> GetUsers();
        bool IsOnline(string user);
    }
}