using System;
using System.Collections.Generic;
using Czeum.DAL;
using Czeum.DAL.Entities;
using Czeum.DTO;
using Czeum.Server.Services.Lobby;

namespace Czeum.Server.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILobbyStorage _lobbyStorage;

        public MessageService(IUnitOfWork unitOfWork, ILobbyStorage lobbyStorage)
        {
            _unitOfWork = unitOfWork;
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

        public Message SendToMatch(int matchId, string message, string sender)
        {
            var match = _unitOfWork.MatchRepository.GetMatchById(matchId);
            if (!match.HasPlayer(sender))
            {
                return null;
            }

            var msg = new Message
            {
                Sender = sender,
                Text = message,
                Timestamp = DateTime.UtcNow
            };
            _unitOfWork.MessageRepository.AddMessage(matchId, msg);
            return msg;
        }

        public List<Message> GetMessagesOfLobby(int lobbyId)
        {
            return _lobbyStorage.GetMessages(lobbyId);
        }

        public List<Message> GetMessagesOfMatch(int matchId)
        {
            return _unitOfWork.MessageRepository.GetMessagesForMatch(matchId);
        }
    }
}