using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.DAL;
using Czeum.DTO;
using IdentityServer4.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Server.Services.Lobby {
	public class LobbyService : ILobbyService
	{
		private readonly ILobbyStorage _lobbyStorage;
		private readonly ApplicationDbContext _context;

		public LobbyService(ILobbyStorage lobbyStorage, ApplicationDbContext context)
		{
			_lobbyStorage = lobbyStorage;
			_context = context;
		}

		public async Task<bool> JoinPlayerToLobbyAsync(string player, int lobbyId)
		{
			var lobby = _lobbyStorage.GetLobby(lobbyId);
			if (lobby == null) {
				throw new ArgumentException("Invalid lobby id");
			}

			var friends = await _context.Friendships
				.Where(f => f.User1.UserName == player || f.User2.UserName == player)
				.Select(f => f.User1.UserName == player ? f.User2.UserName : f.User1.UserName)
				.ToListAsync();
			
			return lobby.JoinGuest(player, friends);
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

		public LobbyData CreateAndAddLobby(Type type, string host, LobbyAccess access, string name)
		{
			if (!type.IsSubclassOf(typeof(LobbyData)))
			{
				throw new ArgumentException("Invalid lobby type.");
			}
			
			var lobby = (LobbyData) Activator.CreateInstance(type);
			lobby.Host = host;
			lobby.Access = access;
			lobby.Name = name.IsNullOrEmpty() ? host + "'s lobby" : name;
			_lobbyStorage.AddLobby(lobby);
			
			return lobby;
		}

		public void AddMessageNow(int lobbyId, Message message)
		{
			message.Timestamp = DateTime.UtcNow;
			_lobbyStorage.AddMessage(lobbyId, message);
		}

		public List<Message> GetMessages(int lobbyId)
		{
			return _lobbyStorage.GetMessages(lobbyId);
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

		public void RemoveLobby(int id)
		{
			_lobbyStorage.RemoveLobby(id);
		}
	}
}
