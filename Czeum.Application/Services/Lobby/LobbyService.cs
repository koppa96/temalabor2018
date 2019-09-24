using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.DAL;
using Czeum.DTO;
using Czeum.DTO.Extensions;
using Czeum.DTO.Lobbies;
using Czeum.DTO.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Application.Services.Lobby {
	public class LobbyService : ILobbyService
	{
		private readonly ILobbyStorage lobbyStorage;
		private readonly ApplicationDbContext context;
		private readonly IMapper mapper;

		public LobbyService(ILobbyStorage lobbyStorage, 
			ApplicationDbContext context,
			IMapper mapper)
		{
			this.lobbyStorage = lobbyStorage;
			this.context = context;
			this.mapper = mapper;
		}

		public async Task JoinPlayerToLobbyAsync(string player, int lobbyId)
		{
			var lobby = lobbyStorage.GetLobby(lobbyId);
			if (lobby == null)
			{
				throw new ArgumentOutOfRangeException(nameof(lobbyId), "Invalid lobby id.");
			}

			var friends = await context.Friendships
				.Where(f => f.User1.UserName == player || f.User2.UserName == player)
				.Select(f => f.User1.UserName == player ? f.User2.UserName : f.User1.UserName)
				.ToListAsync();
			
			lobby.JoinGuest(player, friends);
		}

		public void DisconnectPlayerFromLobby(string player, int lobbyId)
		{
			var lobby = lobbyStorage.GetLobby(lobbyId);
			lobby.DisconnectPlayer(player);

			if (lobby.Empty)
			{
				lobbyStorage.RemoveLobby(lobbyId);
			}			
		}

		public void InvitePlayerToLobby(int lobbyId, string invitingPlayer, string player)
		{
			var lobby = lobbyStorage.GetLobby(lobbyId);
			if (lobby.Host != invitingPlayer)
			{
				throw new UnauthorizedAccessException("Not authorized to invite to this lobby.");
			}

			if (!lobby.InvitedPlayers.Contains(player))
			{
				lobby.InvitedPlayers.Add(player);
			}
		}

		public string KickGuest(int lobbyId, string kickingPlayer)
		{
			var lobby = lobbyStorage.GetLobby(lobbyId);
			if (lobby.Host != kickingPlayer)
			{
				throw new UnauthorizedAccessException("Not authorized to kick a player from this lobby.");
			}

			var guestName = lobby.Guest;
			if (lobby.Guest != null) 
			{
				lobby.DisconnectPlayer(guestName);
			}

			return guestName;
		}

		public LobbyData GetLobbyOfUser(string user) {
			return lobbyStorage.GetLobbyOfUser(user);
		}

		public List<LobbyDataWrapper> GetLobbies()
		{
			return lobbyStorage.GetLobbies().Select(mapper.Map<LobbyDataWrapper>).ToList();
		}

		public void UpdateLobbySettings(LobbyDataWrapper lobbyData, string updatingUser)
		{
			var oldLobby = lobbyStorage.GetLobby(lobbyData.Content.LobbyId);
			if (oldLobby == null)
			{
				throw new ArgumentOutOfRangeException(nameof(lobbyData.Content.LobbyId), "Lobby does not exist.");
			}

			if (updatingUser != oldLobby.Host)
			{
				throw new UnauthorizedAccessException("Not authorized to update this lobby's settings.");
			}

			lobbyData.Content.Host = oldLobby.Host;
			lobbyData.Content.Guest = oldLobby.Guest;
			lobbyData.Content.InvitedPlayers = oldLobby.InvitedPlayers;
			
			lobbyStorage.UpdateLobby(lobbyData.Content);
		}

		public LobbyDataWrapper GetLobby(int lobbyId)
		{
            var lobby = lobbyStorage.GetLobby(lobbyId);
			return mapper.Map<LobbyDataWrapper>(lobby);
		}

		public bool LobbyExists(int lobbyId)
		{
			return lobbyStorage.GetLobby(lobbyId) != null;
		}

		public LobbyDataWrapper CreateAndAddLobby(GameType type, string host, LobbyAccess access, string name)
		{
			var lobbyType = type.GetLobbyType();
			if (!lobbyType.IsSubclassOf(typeof(LobbyData)))
			{
				throw new ArgumentException("Invalid lobby type.");
			}
			
			var lobby = (LobbyData) Activator.CreateInstance(lobbyType);
			lobby.Host = host;
			lobby.Access = access;
			lobby.Name = string.IsNullOrEmpty(name) ? host + "'s lobby" : name;
			lobbyStorage.AddLobby(lobby);
			
			return mapper.Map<LobbyDataWrapper>(lobby);
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
