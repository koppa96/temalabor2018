using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.DAL.Interfaces;
using Czeum.DTO;

namespace Czeum.Server.Services.Lobby {
	public class LobbyService : ILobbyService
	{
		private readonly ILobbyStorage _lobbyStorage;
		private readonly IFriendRepository _repository;

		public LobbyService(ILobbyStorage lobbyStorage, IFriendRepository repository)
		{
			_lobbyStorage = lobbyStorage;
			_repository = repository;
		}

		public void AddLobby(LobbyData lobbyData)
		{
			_lobbyStorage.AddLobby(lobbyData);
		}

		public bool JoinPlayerToLobby(string player, int lobbyId)
		{
			var lobby = _lobbyStorage.GetLobby(lobbyId);
			if (lobby == null) {
				throw new ArgumentException("Invalid lobby id");
			}

			return lobby.JoinGuest(player, _repository.GetFriendsOf(player));
		}

		public void DisconnectPlayerFromLobby(string player, int lobbyId)
		{
			var lobby = _lobbyStorage.GetLobby(lobbyId);
			lobby.DisconnectPlayer(player);

			if (lobby.Empty) {
				_lobbyStorage.RemoveLobby(lobbyId);
			}			
		}

		public void InvitePlayerToLobby(int lobbyId, string player)
		{
			var lobby = _lobbyStorage.GetLobby(lobbyId);

			if (!lobby.InvitedPlayers.Contains(player))
			{
				lobby.InvitedPlayers.Add(player);
			}
		}

		public string KickGuest(int lobbyId)
		{
			var lobby = _lobbyStorage.GetLobby(lobbyId);

			string guestName = lobby.Guest;
			if (lobby.Guest != null) {
				lobby.DisconnectPlayer(guestName);
			}

			return guestName;
		}

		public LobbyData FindUserLobby(string user) {
			return _lobbyStorage.GetLobbyOfUser(user);
		}

		public List<LobbyData> GetLobbies()
		{
			return _lobbyStorage.GetLobbies().ToList();
		}

		public void UpdateLobbySettings(LobbyData lobbyData)
		{
			var oldLobby = _lobbyStorage.GetLobby(lobbyData.LobbyId);
			if (oldLobby == null)
			{
				throw new ArgumentException("Lobby does not exist.");
			}

			lobbyData.Host = oldLobby.Host;
			lobbyData.Guest = oldLobby.Guest;
			lobbyData.InvitedPlayers = oldLobby.InvitedPlayers;
			
			_lobbyStorage.UpdateLobby(lobbyData);
		}

		public LobbyData GetLobby(int lobbyData)
		{
			return _lobbyStorage.GetLobby(lobbyData);
		}

		public bool ValidateModifier(int lobbyId, string modifier)
		{
			return _lobbyStorage.GetLobby(lobbyId).Host == modifier;
		}

		public bool LobbyExists(int lobbyId)
		{
			return _lobbyStorage.GetLobby(lobbyId) != null;
		}

		public LobbyData CreateLobby(Type type)
		{
			if (!type.IsSubclassOf(typeof(LobbyData)))
			{
				throw new ArgumentException("Invalid lobby type.");
			}
			
			return (LobbyData) Activator.CreateInstance(type);
		}

		public void AddMessage(int lobbyId, Message message)
		{
			message.Timestamp = DateTime.Now;
			_lobbyStorage.AddMessage(lobbyId, message);
		}

		public List<Message> GetMessages(int lobbyId)
		{
			return _lobbyStorage.GetMessages(lobbyId);
		}

		public bool ValidateMessageSender(int lobbyId, string sender)
		{
			var lobby = _lobbyStorage.GetLobby(lobbyId);
			return sender == lobby.Host || sender == lobby.Guest;
		}

		public string GetOtherPlayer(int lobbyId, string player)
		{
			var lobby = _lobbyStorage.GetLobby(lobbyId);
			return player == lobby.Host ? lobby.Guest : lobby.Host;
		}

		public void CancelInviteFromLobby(int lobbyId, string player)
		{
			var lobby = _lobbyStorage.GetLobby(lobbyId);
			lobby.InvitedPlayers.Remove(player);
		}
	}
}
