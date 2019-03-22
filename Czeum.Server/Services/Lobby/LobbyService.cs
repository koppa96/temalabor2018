using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.DAL.Interfaces;

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

		public LobbyData AddLobby(LobbyData lobbyData)
		{
			_lobbyStorage.AddLobby(ref lobbyData);
			return lobbyData;
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

		public void UpdateLobbySettings(ref LobbyData lobbyData)
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

		public bool ValidateModifier(string modifier, int lobbyId)
		{
			return _lobbyStorage.GetLobby(lobbyId).Host == modifier;
		}

		public bool LobbyExists(int lobbyId)
		{
			return _lobbyStorage.GetLobby(lobbyId) != null;
		}

		public void CancelInviteFromLobby(int lobbyId, string player)
		{
			var lobby = _lobbyStorage.GetLobby(lobbyId);
			lobby.InvitedPlayers.Remove(player);
		}
	}
}
