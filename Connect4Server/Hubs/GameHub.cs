using Connect4Server.Data;
using Connect4Server.Models.Board;
using Connect4Server.Models.Lobby;
using Connect4Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4Server.Hubs {
	[Authorize]
    public class GameHub : Hub {
		private readonly GameService gameService;
		private readonly LobbyService lobbyService;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SoloQueueService soloQueueService;
		private readonly IHubContext<GameHub> hubContext;

		public GameHub(LobbyService lobbyService, UserManager<ApplicationUser> userManager, GameService gameService, 
						SoloQueueService soloQueueService, IHubContext<GameHub> hubContext) {
			this.lobbyService = lobbyService;
			this.userManager = userManager;
			this.gameService = gameService;
			this.soloQueueService = soloQueueService;
			this.hubContext = hubContext;
		}

		public async Task CreateLobbyAsync(string statusCode) {
			LobbyStatus status = Enum.Parse<LobbyStatus>(statusCode);

			ApplicationUser user = await userManager.FindByNameAsync(Context.User.Identity.Name);
			LobbyModel lobby = lobbyService.CreateLobby(user, status);

			JObject obj = new JObject() {
				{ "host", lobby.Host.UserName },
				{ "guest", lobby.Guest == null ? "null" : lobby.Guest.UserName },
				{ "boardheight", lobby.Settings.BoardHeight },
				{ "boardwidth", lobby.Settings.BoardWidth },
				{ "status", lobby.Settings.Status.ToString() }
			};

			Clients.Caller.SendAsync("LobbyCreated", obj.ToString());
		}

		public void CreateMatch(int lobbyId) {
			try {
				Match newMatch = gameService.CreateMatchFromLobby(lobbyService.Lobbies[lobbyId]);
				lobbyService.DeleteLobby(lobbyId);
				Clients.Caller.SendAsync("MatchCreated", newMatch);
				Clients.User(newMatch.Player2.UserName).SendAsync("MatchCreated", newMatch);
				Clients.All.SendAsync("UpdateLobbyList", lobbyService.Lobbies);
			} catch (ArgumentException) {
				Clients.Caller.SendAsync("NotEnoughPlayersHandler");
			}
		}

		public void LobbySettingsChanged(int lobbyId, LobbySettings settings) {
			LobbyModel lobby = lobbyService.Lobbies[lobbyId];
			lobby.Settings = settings;

			if (lobby.Guest != null) {
				Clients.User(lobby.Guest.UserName).SendAsync("LobbySettingsChanged", settings);
			}
		}

		public void SendInvitationTo(int lobbyId, string user) {
			Clients.User(user).SendAsync("GetInvitationTo", lobbyId);
		}

		public async void JoinLobbyAsync(int lobbyId) {
			ApplicationUser user = await userManager.FindByNameAsync(Context.User.Identity.Name);

			if (lobbyService.JoinPlayerToLobby(user, lobbyId)) {
				Clients.Caller.SendAsync("JoinedToLobby", lobbyService.Lobbies[lobbyId]);
			} else {
				Clients.Caller.SendAsync("FailedToJoinLobby");
			}
		}

		public async void PlaceItemAsync(int column, int matchId) {
			Match match = gameService.GetMatchById(matchId);
			ApplicationUser player = await userManager.FindByNameAsync(Context.User.Identity.Name);

			Board board = Board.Parse(match.BoardData);
		}
	}
}
