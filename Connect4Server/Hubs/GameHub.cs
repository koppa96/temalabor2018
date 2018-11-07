using Connect4Server.Data;
using Connect4Server.Models.Board;
using Connect4Server.Models.Lobby;
using Connect4Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
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

		public GameHub(LobbyService lobbyService, UserManager<ApplicationUser> userManager, GameService gameService, SoloQueueService soloQueueService) {
			this.lobbyService = lobbyService;
			this.userManager = userManager;
			this.gameService = gameService;
			this.soloQueueService = soloQueueService;
		}

		public async void CreateLobbyAsync(string statusCode) {
			LobbyStatus status = Enum.Parse<LobbyStatus>(statusCode);

			ApplicationUser user = await userManager.FindByNameAsync(Context.User.Identity.Name);
			int lobbyId = lobbyService.CreateLobby(user, status);

			Clients.Caller.SendAsync("LobbyCreated", lobbyId);
		}

		public void CreateMatch(int lobbyId) {
			Match newMatch = gameService.CreateMatchFromLobby(lobbyService.Lobbies[lobbyId]);

			Clients.User(newMatch.Player1.UserName).SendAsync("MatchCreated", newMatch);
			Clients.User(newMatch.Player2.UserName).SendAsync("MatchCreated", newMatch);
			//TODO Lobby lista frissítés
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
