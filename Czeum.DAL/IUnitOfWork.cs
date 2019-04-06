using System;
using Czeum.DAL.Entities;
using Czeum.DAL.Interfaces;

namespace Czeum.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        IMatchRepository MatchRepository { get; }
        IMessageRepository MessageRepository { get; }
        IFriendRepository FriendRepository { get; }
        IBoardRepository BoardRepository { get; }
        
        void Save();
    }
}