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
			LobbyModel lobby = _lobbyService.CreateLobby(Context.UserIdentifier, status);

			Clients.Caller.SendAsync("LobbyCreated", lobby.Data);
			Clients.All.SendAsync("LobbyAddedHandler", lobby.Data);
		}

		public async Task CreateMatchAsync(int lobbyId) {
			try {
				LobbyModel lobby = _lobbyService.FindLobbyById(lobbyId);
				ApplicationUser player1 = await _userManager.FindByNameAsync(lobby.Data.Host);
				ApplicationUser player2 = await _userManager.FindByNameAsync(lobby.Data.Guest);
				Match newMatch = _gameService.CreateMatch(player1, player2, lobby.Data.BoardHeight, lobby.Data.BoardWidth);
				_lobbyService.DeleteLobby(lobbyId);
				MatchDto dto = new MatchDto {
					MatchId = newMatch.MatchId,
					OtherPlayer = lobby.Data.Guest,
					BoardData = newMatch.BoardData,
					State = "YourTurn"
				};

				await Clients.Caller.SendAsync("MatchCreated", dto);
				dto.OtherPlayer = lobby.Data.Host;
				dto.State = "EnemyTurn";
				await Clients.User(lobby.Data.Guest).SendAsync("MatchCreated", dto);
				await Clients.All.SendAsync("LobbyDeletedHandler", lobbyId);
			} catch (ArgumentException) {
				await Clients.Caller.SendAsync("NotEnoughPlayersHandler");
			}
		}

		public void LobbySettingsChanged(LobbyData data) {
			LobbyModel lobby = _lobbyService.FindLobbyById(data.LobbyId);
			if (Context.User.Identity.Name != lobby.Data.Host) {
				Clients.Caller.SendAsync("CannotSetOtherLobby");
				return;
			}

			lobby.Data.BoardHeight = data.BoardHeight;
			lobby.Data.BoardWidth = data.BoardWidth;
			lobby.Data.Status = data.Status;

			if (lobby.Data.Guest != null) {
				Clients.User(lobby.Data.Guest).SendAsync("LobbySettingsChanged", data);
			}
		}

		public void SendInvitationTo(int lobbyId, string user) {
			_lobbyService.InvitePlayerToLobby(lobbyId, user);
			Clients.User(user).SendAsync("GetInvitationTo", lobbyId);
		}

		public void JoinLobby(int lobbyId) {
			if (_lobbyService.JoinPlayerToLobby(Context.UserIdentifier, lobbyId)) {
				LobbyModel lobbyModel = _lobbyService.FindLobbyById(lobbyId);

				Clients.Caller.SendAsync("JoinedToLobby", lobbyModel.Data);
				Clients.User(lobbyModel.Data.Host)
					.SendAsync("PlayerJoinedToLobby", Context.UserIdentifier);
			} else {
				Clients.Caller.SendAsync("FailedToJoinLobby");
			}
		}

		public List<MatchDto> GetMatches() {
			return _gameService.GetMatchesOf(Context.UserIdentifier);
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

				MatchDto dto = new MatchDto {
					MatchId = match.MatchId,
					OtherPlayer = player2.UserName,
					BoardData = match.BoardData,
					State = "YourTurn"
				};

				await Clients.User(player1.UserName).SendAsync("MatchCreated", dto);
				dto.State = "EnemyTurn";
				dto.OtherPlayer = player1.UserName;
				await Clients.User(player2.UserName).SendAsync("MatchCreated", dto);
			}
		}

		public void LeaveSoloQue() {
			_soloQueueService.QueingPlayers.Remove(Context.UserIdentifier);
		}

		public void KickGuest(int lobbyId) {
			LobbyModel model = _lobbyService.FindLobbyById(lobbyId);

			if (Context.UserIdentifier != model.Data.Host) {
				Clients.Caller.SendAsync("CannotSetOtherLobby");
				return;
			}

			string kickedGuest = _lobbyService.KickGuest(lobbyId);
			Clients.Caller.SendAsync("GuestKicked");
			Clients.User(kickedGuest).SendAsync("YouHaveBeenKicked");
		}

		public List<LobbyData> GetLobbies() {
			return _lobbyService.GetLobbyData();
		}
	}
}
