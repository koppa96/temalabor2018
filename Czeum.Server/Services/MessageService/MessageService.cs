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

        public bool SendToLobby(int lobbyId, Message message, string sender)
        {
            var lobby = _lobbyStorage.GetLobby(lobbyId);
            if (lobby == null || (lobby.Host != sender && lobby.Guest != sender) || message.Sender != sender)
            {
                return false;
            }
            
            message.Timestamp = DateTime.UtcNow;
            _lobbyStorage.AddMessage(lobbyId, message);
            return true;
        }

        public bool SendToMatch(int matchId, Message message, string sender)
        {
            var match = _unitOfWork.MatchRepository.GetMatchById(matchId);
            if (!match.HasPlayer(sender) && message.Sender != sender)
            {
                return false;
            }

            message.Timestamp = DateTime.UtcNow;
            _unitOfWork.MessageRepository.AddMessage(matchId, message);
            return true;
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