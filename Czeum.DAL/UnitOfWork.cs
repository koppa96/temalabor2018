using System;
using Czeum.DAL.Entities;
using Czeum.DAL.Interfaces;
using Czeum.DAL.Repositories;

namespace Czeum.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private IMatchRepository _matchRepository;
        private IMessageRepository _messageRepository;
        private IFriendRepository _friendRepository;
        private IBoardRepository _boardRepository;

        public IMatchRepository MatchRepository => 
            _matchRepository ?? (_matchRepository = new MatchRepository(_context));

        public IMessageRepository MessageRepository =>
            _messageRepository ?? (_messageRepository = new MessageRepository(_context));

        public IFriendRepository FriendRepository =>
            _friendRepository ?? (_friendRepository = new FriendRepository(_context));

        public IBoardRepository BoardRepository =>
            _boardRepository ?? (_boardRepository = new BoardRepository<SerializedBoard>(_context));

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}