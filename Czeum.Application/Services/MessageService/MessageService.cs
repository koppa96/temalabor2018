using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Application.Services.Lobby;
using Czeum.DAL;
using Czeum.DAL.Entities;
using Czeum.DAL.Extensions;
using Czeum.DTO;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Application.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext context;
        private readonly ILobbyStorage lobbyStorage;

        public MessageService(ApplicationDbContext context, ILobbyStorage lobbyStorage)
        {
            this.context = context;
            this.lobbyStorage = lobbyStorage;
        }

        public Message SendToLobby(Guid lobbyId, string message, string sender)
        {
            var lobby = lobbyStorage.GetLobby(lobbyId);
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
            lobbyStorage.AddMessage(lobbyId, msg);
            return msg;
        }

        public async Task<Message> SendToMatchAsync(Guid matchId, string message, string sender)
        {
            var match = await context.Matches.FindAsync(matchId);
            if (match == null || !match.HasPlayer(sender))
            {
                return null;
            }

            var senderUser = await context.Users.SingleAsync(u => u.UserName == sender);
            var storedMessage = new StoredMessage
            {
                Sender = senderUser,
                Match = match,
                Text = message,
                Timestamp = DateTime.UtcNow
            };
            context.Messages.Add(storedMessage);
            await context.SaveChangesAsync();
            
            return storedMessage.ToMessage();
        }

        public List<Message> GetMessagesOfLobby(Guid lobbyId)
        {
            return lobbyStorage.GetMessages(lobbyId);
        }

        public async Task<List<Message>> GetMessagesOfMatchAsync(Guid matchId)
        {
            return await context.Messages.Where(m => m.Match.Id == matchId)
                .Select(m => m.ToMessage())
                .ToListAsync();
        }
    }
}