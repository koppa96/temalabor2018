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

		/// <summary>
		/// Creates a new Lobby instance that is hosted by the player that called this method
		/// </summary>
		/// <param name="statusCode">Public or Private, depending on the desired lobby status</param>
		/// <returns></returns>
		public void CreateLobby(string statusCode) {
			LobbyStatus status = Enum.Parse<LobbyStatus>(statusCode);
			int lobby = lobbyService.CreateLobby(Context.User.Identity.Name, status);

			Clients.Caller.SendAsync("LobbyCreated", lobby);
			Clients.All.SendAsync("UpdateLobbyList", lobbyService.Lobbies);
		}

		public async Task CreateMatchAsync(int lobbyId) {
			try {
				LobbyModel lobby = lobbyService.Lobbies[lobbyId];
				ApplicationUser player1 = await userManager.FindByNameAsync(lobby.Host);
				ApplicationUser player2 = await userManager.FindByNameAsync(lobby.Guest);
				Match newMatch = gameService.CreateMatch(player1, player2, lobby.BoardHeight, lobby.BoardWidth);
				lobbyService.DeleteLobby(lobbyId);

				Clients.Caller.SendAsync("MatchCreated", newMatch);
				Clients.User(newMatch.Player2.UserName).SendAsync("MatchCreated", newMatch);
				Clients.All.SendAsync("UpdateLobbyList", lobbyService.Lobbies);
			} catch (ArgumentException) {
				Clients.Caller.SendAsync("NotEnoughPlayersHandler");
			}
		}

		public void LobbySettingsChanged(int lobbyId, int newHeight, int newWidth, string newStatus) {
			LobbyModel lobby = lobbyService.Lobbies[lobbyId];
			lobby.BoardHeight = newHeight;
			lobby.BoardWidth = newWidth;
			lobby.Status = Enum.Parse<LobbyStatus>(newStatus);

			if (lobby.Guest != null) {
				Clients.User(lobby.Guest).SendAsync("LobbySettingsChanged", newHeight, newWidth, newStatus);
			}
		}

		public void SendInvitationTo(int lobbyId, string user) {
			Clients.User(user).SendAsync("GetInvitationTo", lobbyId);
		}

		public void JoinLobbyAsync(int lobbyId) {
			if (lobbyService.JoinPlayerToLobby(Context.User.Identity.Name, lobbyId)) {
				Clients.Caller.SendAsync("JoinedToLobby", lobbyService.Lobbies[lobbyId]);
			} else {
				Clients.Caller.SendAsync("FailedToJoinLobby");
			}
		}

		public async Task PlaceItemAsync(int column, int matchId) {
			Match match = gameService.GetMatchById(matchId);
			ApplicationUser player = await userManager.FindByNameAsync(Context.User.Identity.Name);

			Board board = Board.Parse(match.BoardData);

			//TODO
		}
	}
}
