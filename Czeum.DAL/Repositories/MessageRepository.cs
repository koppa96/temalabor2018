using System;
using System.Collections.Generic;
using System.Linq;
using Czeum.DAL.Entities;
using Czeum.DAL.Interfaces;
using Czeum.DTO;

namespace Czeum.DAL.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public void AddMessage(int matchId, Message message)
        {
            var storedMessage = new StoredMessage
            {
                Text = message.Text,
                Sender = _context.Users.SingleOrDefault(u => u.UserName == message.Sender),
                Timestamp = message.Timestamp,
                Match = _context.Matches.Find(matchId)
            };

            if (storedMessage.Sender == null)
            {
                throw new ArgumentException("The sender must be a valid user.");
            }

            _context.Messages.Add(storedMessage);
        }

        public List<Message> GetMessagesForMatch(int matchId)
        {
            return _context.Messages.Where(m => m.Match.MatchId == matchId)
                .Select(m => new Message
                {
                    Sender = m.Sender.UserName,
                    Text = m.Text,
                    Timestamp = m.Timestamp
                }).ToList();
        }
    }
}