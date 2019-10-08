using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Czeum.Application.Extensions;
using Czeum.Application.Services.Lobby;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Abstractions.Lobbies;
using Czeum.Core.DTOs.Extensions;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.Enums;
using Czeum.Core.Services;
using Czeum.DAL;
using Czeum.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Application.Services {
	public class LobbyService : ILobbyService
	{
		private readonly ILobbyStorage lobbyStorage;
		private readonly CzeumContext context;
		private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly ISoloQueueService soloQueueService;
        private readonly INotificationService notificationService;

        public LobbyService(ILobbyStorage lobbyStorage, 
			CzeumContext context,
			IMapper mapper,
            IIdentityService identityService,
            ISoloQueueService soloQueueService,
			INotificationService notificationService)
		{
			this.lobbyStorage = lobbyStorage;
			this.context = context;
			this.mapper = mapper;
            this.identityService = identityService;
            this.soloQueueService = soloQueueService;
            this.notificationService = notificationService;
		}

		public async Task<LobbyDataWrapper> JoinToLobbyAsync(Guid lobbyId)
		{
            var currentUser = identityService.GetCurrentUserName();
            var userLobby = lobbyStorage.GetLobbyOfUser(currentUser);
            if (userLobby != null || soloQueueService.IsQueuing(currentUser))
            {
                throw new InvalidOperationException("You can only join a lobby if you are not queuing and not in an other lobby.");
            }

			var lobby = lobbyStorage.GetLobby(lobbyId);

			var friends = await context.Friendships
				.Where(f => f.User1.UserName == currentUser || f.User2.UserName == currentUser)
				.Select(f => f.User1.UserName == currentUser ? f.User2.UserName : f.User1.UserName)
				.ToListAsync();

			var wrapper = mapper.Map<LobbyDataWrapper>(lobby);
			
			lobby.JoinGuest(currentUser, friends);
			await notificationService.NotifyAllExceptAsync(currentUser,
				client => client.LobbyChanged(wrapper));

			return wrapper;
		}

		public Task DisconnectFromCurrentLobbyAsync()
		{
            var currentUser = identityService.GetCurrentUserName();
            return DisconnectPlayerFromLobby(currentUser);
		}

        public async Task DisconnectPlayerFromLobby(string username)
        {
            var currentLobby = lobbyStorage.GetLobbyOfUser(username);
            if (currentLobby != null)
            {
                currentLobby.DisconnectPlayer(username);
                if (currentLobby.Empty)
                {
                    lobbyStorage.RemoveLobby(currentLobby.Id);
                    await notificationService.NotifyAllAsync(client => client.LobbyDeleted(currentLobby.Id));
                }
                else
                {
	                await notificationService.NotifyAllAsync(client =>
		                client.LobbyChanged(mapper.Map<LobbyDataWrapper>(currentLobby)));
                }
            }
            else
            {
                throw new InvalidOperationException("You are not in a lobby.");
            }
        }

        public async Task<LobbyDataWrapper> InvitePlayerToLobby(Guid lobbyId, string player)
		{
			var lobby = lobbyStorage.GetLobby(lobbyId);
			if (lobby.Host != identityService.GetCurrentUserName())
			{
				throw new UnauthorizedAccessException("Not authorized to invite to this lobby.");
			}

			if (lobby.InvitedPlayers.Contains(player))
			{
				throw new InvalidOperationException("This player has already been invited.");
			}

			lobby.InvitedPlayers.Add(player);
			lobby.LastModified = DateTime.UtcNow;

			var wrapper = mapper.Map<LobbyDataWrapper>(lobby);
			await notificationService.NotifyAsync(player,
				client => client.ReceiveLobbyInvite(wrapper));

			await notificationService.NotifyAllExceptAsync(identityService.GetCurrentUserName(),
				client => client.LobbyChanged(wrapper));

			return wrapper;
		}

		public async Task<LobbyDataWrapper> KickGuestAsync(Guid lobbyId, string guestName)
		{
			var lobby = lobbyStorage.GetLobby(lobbyId);
			if (lobby.Host != identityService.GetCurrentUserName())
			{
				throw new UnauthorizedAccessException("Not authorized to kick a player from this lobby.");
			}

			await DisconnectPlayerFromLobby(guestName);
			await notificationService.NotifyAsync(guestName,
				client => client.KickedFromLobby());

			return mapper.Map<LobbyDataWrapper>(lobby);
		}

		public LobbyData? GetLobbyOfUser(string user) {
			return lobbyStorage.GetLobbyOfUser(user);
		}

		public List<LobbyDataWrapper> GetLobbies()
		{
			return lobbyStorage.GetLobbies().Select(mapper.Map<LobbyDataWrapper>).ToList();
		}

		public async Task<LobbyDataWrapper> UpdateLobbySettingsAsync(LobbyDataWrapper lobbyData)
		{
			var currentUserName = identityService.GetCurrentUserName();
			var oldLobby = lobbyStorage.GetLobby(lobbyData.Content.Id);
			if (oldLobby == null)
			{
				throw new ArgumentOutOfRangeException(nameof(lobbyData.Content.Id), "Lobby does not exist.");
			}

			if (currentUserName != oldLobby.Host)
			{
				throw new UnauthorizedAccessException("Not authorized to update this lobby's settings.");
			}

			if (!lobbyData.Content.ValidateSettings())
			{
				throw new InvalidOperationException("Invalid settings for this lobby.");
			}

			lobbyData.Content.Host = oldLobby.Host;
			lobbyData.Content.Guests = oldLobby.Guests;
			lobbyData.Content.InvitedPlayers = oldLobby.InvitedPlayers;
            lobbyData.Content.Created = oldLobby.Created;
            lobbyData.Content.LastModified = DateTime.UtcNow;
			
			lobbyStorage.UpdateLobby(lobbyData.Content);
			var updatedLobby = mapper.Map<LobbyDataWrapper>(lobbyStorage.GetLobby(lobbyData.Content.Id));

			await notificationService.NotifyAllExceptAsync(currentUserName,
				client => client.LobbyChanged(updatedLobby));

			return updatedLobby;
		}

		public LobbyDataWrapper GetLobby(Guid lobbyId)
		{
            var lobby = lobbyStorage.GetLobby(lobbyId);
			return mapper.Map<LobbyDataWrapper>(lobby);
		}

		public bool LobbyExists(Guid lobbyId)
		{
            return lobbyStorage.LobbyExitsts(lobbyId);
		}

		public async Task<LobbyDataWrapper> CreateAndAddLobbyAsync(GameType type, LobbyAccess access, string name)
		{
            var currentUser = identityService.GetCurrentUserName();
            if (lobbyStorage.GetLobbyOfUser(currentUser) != null)
            {
                throw new InvalidOperationException("To create a new lobby, leave your current lobby first.");
            }

			var lobbyType = type.GetLobbyType();
			if (!lobbyType.IsSubclassOf(typeof(LobbyData)))
			{
				throw new ArgumentException("Invalid lobby type.");
			}
			
			var lobby = (LobbyData) Activator.CreateInstance(lobbyType)!;
			lobby.Host = currentUser;
			lobby.Access = access;
			lobby.Name = string.IsNullOrEmpty(name) ? $"{currentUser}'s {type.ToString()} lobby" : name;
			lobbyStorage.AddLobby(lobby);
			var wrapper = mapper.Map<LobbyDataWrapper>(lobby);

			await notificationService.NotifyAllExceptAsync(currentUser,
				client => client.LobbyAdded(wrapper));

			return wrapper;
		}

		public List<Message> GetMessages(Guid lobbyId)
		{
			return lobbyStorage.GetMessages(lobbyId);
		}

		public async Task<LobbyDataWrapper> CancelInviteFromLobby(Guid lobbyId, string player)
        { 
			var lobby = lobbyStorage.GetLobby(lobbyId);
            if (identityService.GetCurrentUserName() != lobby.Host)
            {
                throw new UnauthorizedAccessException("Only the host can modify the lobby.");
            }

			lobby.InvitedPlayers.Remove(player);

			var wrapper = mapper.Map<LobbyDataWrapper>(lobby);
			await notificationService.NotifyAllExceptAsync(identityService.GetCurrentUserName(),
				client => client.LobbyChanged(wrapper));

			return wrapper;
        }

		public void RemoveLobby(Guid id)
		{
			lobbyStorage.RemoveLobby(id);
		}

		public IEnumerable<string> GetOthersInLobby(Guid lobbyId)
		{
			var lobby = lobbyStorage.GetLobby(lobbyId);
			var currentUser = identityService.GetCurrentUserName();

			return lobby.Guests.Append(lobby.Host)
				.Where(u => u != currentUser);
		}
    }
}
