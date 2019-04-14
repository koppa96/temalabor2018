using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.DAL;
using Czeum.DAL.Entities;
using Czeum.DTO;
using Czeum.Server.Services.Lobby;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Server.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILobbyStorage _lobbyStorage;

        public MessageService(ApplicationDbContext context, ILobbyStorage lobbyStorage)
        {
            _context = context;
            _lobbyStorage = lobbyStorage;
        }

        public Message SendToLobby(int lobbyId, string message, string sender)
        {
            var lobby = _lobbyStorage.GetLobby(lobbyId);
            if (lobby == null || lobby.Host != sender && lobby.Guest != sender)
            {
                return null;
            }

            var msg = new Message
            {
                Sender = sender,
                Text = message,
                Timestamp = DateTime.UtcNow
            };
            _lobbyStorage.AddMessage(lobbyId, msg);
            return msg;
        }

        public async Task<Message> SendToMatchAsync(int matchId, string message, string sender)
        {
            var match = await _context.Matches.FindAsync(matchId);
            if (match == null || !match.HasPlayer(sender))
            {
                return null;
            }

            var senderUser = await _context.Users.SingleAsync(u => u.UserName == sender);
            var storedMessage = new StoredMessage
            {
                Sender = senderUser,
                Match = match,
                Text = message,
                Timestamp = DateTime.UtcNow
            };
            _context.Messages.Add(storedMessage);
            await _context.SaveChangesAsync();
            
            return storedMessage.ToMessage();
        }

        public List<Message> GetMessagesOfLobby(int lobbyId)
        {
            return _lobbyStorage.GetMessages(lobbyId);
        }

        public async Task<List<Message>> GetMessagesOfMatchAsync(int matchId)
        {
            return await _context.Messages.Where(m => m.Match.MatchId == matchId)
                .Select(m => m.ToMessage())
                .ToListAsync();
        }
    }
}