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
using Connect4Dtos;

namespace Connect4Server.Hubs {
	[Authorize]
    public class GameHub : Hub {
		private readonly GameService _gameService;
		private readonly LobbyService _lobbyService;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SoloQueueService _soloQueueService;

		public GameHub(LobbyService lobbyService, UserManager<ApplicationUser> userManager, GameService gameService, 
						SoloQueueService soloQueueService, IHubContext<GameHub> hubContext) {
			_lobbyService = lobbyService;
			_userManager = userManager;
			_gameService = gameService;
			_soloQueueService = soloQueueService;
		}

		/// <summary>
		/// Creates a new Lobby instance that is hosted by the player that called this method
		/// </summary>
		/// <param name="statusCode">Public or Private, depending on the desired lobby status</param>
		/// <returns></returns>
		public void CreateLobby(string statusCode) {
			LobbyStatus status = Enum.Parse<LobbyStatus>(statusCode);
			int lobbyId = _lobbyService.CreateLobby(Context.User.Identity.Name, status);
			LobbyModel model = _lobbyService.Lobbies[lobbyId];

			Clients.Caller.SendAsync("LobbyCreated", lobbyId);
			Clients.All.SendAsync("LobbyAddedHandler", lobbyId, model.Host, model.Status.ToString());
		}

		public async Task CreateMatchAsync(int lobbyId) {
			try {
				LobbyModel lobby = _lobbyService.Lobbies[lobbyId];
				ApplicationUser player1 = await _userManager.FindByNameAsync(lobby.Host);
				ApplicationUser player2 = await _userManager.FindByNameAsync(lobby.Guest);
				Match newMatch = _gameService.CreateMatch(player1, player2, lobby.BoardHeight, lobby.BoardWidth);
				_lobbyService.DeleteLobby(lobbyId);

				Clients.Caller.SendAsync("MatchCreated", newMatch.MatchId);
				Clients.User(lobby.Guest).SendAsync("MatchCreated", newMatch.MatchId);
				Clients.All.SendAsync("LobbyDeletedHandler", lobbyId);
			} catch (ArgumentException) {
				Clients.Caller.SendAsync("NotEnoughPlayersHandler");
			}
		}

		public void LobbySettingsChanged(int lobbyId, int newHeight, int newWidth, string newStatus) {
			LobbyModel lobby = _lobbyService.Lobbies[lobbyId];
			if (Context.User.Identity.Name != lobby.Host) {
				Clients.Caller.SendAsync("CannotSetOtherLobby");
				return;
			}

			lobby.BoardHeight = newHeight;
			lobby.BoardWidth = newWidth;
			lobby.Status = Enum.Parse<LobbyStatus>(newStatus);

			if (lobby.Guest != null) {
				Clients.User(lobby.Guest).SendAsync("LobbySettingsChanged", newHeight, newWidth, newStatus);
			}
		}

		public void SendInvitationTo(int lobbyId, string user) {
			_lobbyService.InvitePlayerToLobby(lobbyId, user);
			Clients.User(user).SendAsync("GetInvitationTo", lobbyId);
		}

		public void JoinLobby(int lobbyId) {
			if (_lobbyService.JoinPlayerToLobby(Context.User.Identity.Name, lobbyId)) {
				Clients.Caller.SendAsync("JoinedToLobby", _lobbyService.Lobbies[lobbyId]);
				Clients.User(_lobbyService.Lobbies[lobbyId].Host)
					.SendAsync("PlayerJoinedToLobby", Context.User.Identity.Name);
			} else {
				Clients.Caller.SendAsync("FailedToJoinLobby");
			}
		}

		public List<MatchDto> GetMatches() {
			return _gameService.GetMatchesOf(Context.User.Identity.Name);
		}

		public string GetBoardOfMatch(int matchId) {
			return _gameService.GetBoardOfMatch(matchId);
		}

		public void PlaceItem(int matchId, int column) {
			Match match = _gameService.GetMatchById(matchId);
			if (match == null) {
				Clients.Caller.SendAsync("IncorrectMatchIdHandler");
				return;
			}

			string otherPlayer = _gameService.GetOtherPlayer(matchId, Context.User.Identity.Name).UserName;
			switch (_gameService.PlaceItemToColumn(matchId, column, Context.User.Identity.Name)) {
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

		public async Task JoinSoloQueAsync() {
			_soloQueueService.QueingPlayers.Add(Context.User.Identity.Name);

			if (_soloQueueService.QueingPlayers.Count >= 2) {
				ApplicationUser player1 = await _userManager.FindByNameAsync(_soloQueueService.QueingPlayers[0]);
				ApplicationUser player2 = await _userManager.FindByNameAsync(_soloQueueService.QueingPlayers[1]);

				Match match = _gameService.CreateMatch(player1, player2, 6, 7);
				_soloQueueService.QueingPlayers.Remove(player1.UserName);
				_soloQueueService.QueingPlayers.Remove(player2.UserName);

				Clients.User(player1.UserName).SendAsync("MatchCreated", match.MatchId);
				Clients.User(player2.UserName).SendAsync("MatchCreated", match.MatchId);
			}
		}

		public void LeaveSoloQue() {
			_soloQueueService.QueingPlayers.Remove(Context.User.Identity.Name);
		}

		public void KickGuest(int lobbyId) {
			LobbyModel model = _lobbyService.Lobbies[lobbyId];

			if (Context.User.Identity.Name == model.Guest) {
				Clients.Caller.SendAsync("CannotKickSelf");
				return;
			}

			if (Context.User.Identity.Name != model.Host) {
				Clients.Caller.SendAsync("CannotSetOtherLobby");
				return;
			}

			string kickedGuest = _lobbyService.KickGuest(lobbyId);
			Clients.Caller.SendAsync("GuestKicked");
			Clients.User(kickedGuest).SendAsync("YouHaveBeenKicked");
		}
	}
}
