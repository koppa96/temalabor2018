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

		public GameHub(LobbyService lobbyService, UserManager<ApplicationUser> userManager, GameService gameService, 
						SoloQueueService soloQueueService, IHubContext<GameHub> hubContext) {
			this.lobbyService = lobbyService;
			this.userManager = userManager;
			this.gameService = gameService;
			this.soloQueueService = soloQueueService;
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

		public void JoinLobby(int lobbyId) {
			if (lobbyService.JoinPlayerToLobby(Context.User.Identity.Name, lobbyId)) {
				Clients.Caller.SendAsync("JoinedToLobby", lobbyService.Lobbies[lobbyId]);
			} else {
				Clients.Caller.SendAsync("FailedToJoinLobby");
			}
		}

		public void PlaceItem(int matchId, int column) {
			Match match = gameService.GetMatchById(matchId);
			if (match == null) {
				Clients.Caller.SendAsync("IncorrectMatchIdHandler");
				return;
			}

			string otherPlayer = match.Player1.UserName == Context.User.Identity.Name
				? match.Player2.UserName
				: match.Player1.UserName;
			switch (gameService.PlaceItemToColumn(matchId, column, Context.User.Identity.Name)) {
				case PlacementResult.ColumnFull:
					Clients.Caller.SendAsync("ColumnFullHandler");
					break;
				case PlacementResult.MatchNotRunning:
					Clients.Caller.SendAsync("MatchFinishedHandler");
					break;
				case PlacementResult.NotYourTurn:
					Clients.Caller.SendAsync("NotYourTurnHandler");
					break;
				case PlacementResult.Success:
					Clients.Caller.SendAsync("SuccessfulPlacement", column);
					Clients.User(otherPlayer).SendAsync("SuccessfulEnemyPlacement", column);
					break;
				case PlacementResult.Victory:
					Clients.Caller.SendAsync("VictoryHandler", column);
					Clients.User(otherPlayer).SendAsync("EnemyVictoryHandler", column);
					break;
			}
		}
	}
}
