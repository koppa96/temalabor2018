using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.DAL;
using Czeum.DTO;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Application.Services.Lobby {
	public class LobbyService : ILobbyService
	{
		private readonly ILobbyStorage lobbyStorage;
		private readonly ApplicationDbContext context;

		public LobbyService(ILobbyStorage lobbyStorage, ApplicationDbContext context)
		{
			this.lobbyStorage = lobbyStorage;
			this.context = context;
		}

		public async Task<bool> JoinPlayerToLobbyAsync(string player, int lobbyId)
		{
			var lobby = lobbyStorage.GetLobby(lobbyId);
			if (lobby == null) {
				throw new ArgumentException("Invalid lobby id");
			}

			var friends = await context.Friendships
				.Where(f => f.User1.UserName == player || f.User2.UserName == player)
				.Select(f => f.User1.UserName == player ? f.User2.UserName : f.User1.UserName)
				.ToListAsync();
			
			return lobby.JoinGuest(player, friends);
		}

		public void DisconnectPlayerFromLobby(string player, int lobbyId)
		{
			var lobby = lobbyStorage.GetLobby(lobbyId);
			lobby.DisconnectPlayer(player);

			if (lobby.Empty) {
				lobbyStorage.RemoveLobby(lobbyId);
			}			
		}

		public void InvitePlayerToLobby(int lobbyId, string player)
		{
			var lobby = lobbyStorage.GetLobby(lobbyId);

			if (!lobby.InvitedPlayers.Contains(player))
			{
				lobby.InvitedPlayers.Add(player);
			}
		}

		public string KickGuest(int lobbyId)
		{
			var lobby = lobbyStorage.GetLobby(lobbyId);

			string guestName = lobby.Guest;
			if (lobby.Guest != null) {
				lobby.DisconnectPlayer(guestName);
			}

			return guestName;
		}

		public LobbyData FindUserLobby(string user) {
			return lobbyStorage.GetLobbyOfUser(user);
		}

		public List<LobbyData> GetLobbies()
		{
			return lobbyStorage.GetLobbies().ToList();
		}

		public void UpdateLobbySettings(LobbyData lobbyData)
		{
			var oldLobby = lobbyStorage.GetLobby(lobbyData.LobbyId);
			if (oldLobby == null)
			{
				throw new ArgumentException("Lobby does not exist.");
			}

			lobbyData.Host = oldLobby.Host;
			lobbyData.Guest = oldLobby.Guest;
			lobbyData.InvitedPlayers = oldLobby.InvitedPlayers;
			
			lobbyStorage.UpdateLobby(lobbyData);
		}

		public LobbyData GetLobby(int lobbyData)
		{
			return lobbyStorage.GetLobby(lobbyData);
		}

		public bool ValidateModifier(int lobbyId, string modifier)
		{
			return lobbyStorage.GetLobby(lobbyId).Host == modifier;
		}

		public bool LobbyExists(int lobbyId)
		{
			return lobbyStorage.GetLobby(lobbyId) != null;
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
			lobby.Name = string.IsNullOrEmpty(name) ? host + "'s lobby" : name;
			lobbyStorage.AddLobby(lobby);
			
			return lobby;
		}

		public void AddMessageNow(int lobbyId, Message message)
		{
			message.Timestamp = DateTime.UtcNow;
			lobbyStorage.AddMessage(lobbyId, message);
		}

		public List<Message> GetMessages(int lobbyId)
		{
			return lobbyStorage.GetMessages(lobbyId);
		}

		public string GetOtherPlayer(int lobbyId, string player)
		{
			var lobby = lobbyStorage.GetLobby(lobbyId);
			return player == lobby.Host ? lobby.Guest : lobby.Host;
		}

		public void CancelInviteFromLobby(int lobbyId, string player)
		{
			//TODO: Check the inviting player's identity
			var lobby = lobbyStorage.GetLobby(lobbyId);
			lobby.InvitedPlayers.Remove(player);
		}

		public void RemoveLobby(int id)
		{
			lobbyStorage.RemoveLobby(id);
		}
	}
}
