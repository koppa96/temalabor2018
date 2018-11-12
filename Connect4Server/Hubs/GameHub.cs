using Connect4Server.Data;
using Connect4Server.Models.Board;
using Connect4Server.Models.Lobby;
using Connect4Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Connect4Dtos;
using Microsoft.Extensions.Logging;

namespace Connect4Server.Hubs {
	[Authorize]
    public class GameHub : Hub<IConnect4Client> {
		private readonly GameService _gameService;
		private readonly LobbyService _lobbyService;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SoloQueueService _soloQueueService;
		private readonly ILogger _logger;

		public GameHub(LobbyService lobbyService, UserManager<ApplicationUser> userManager, GameService gameService, 
						SoloQueueService soloQueueService, IHubContext<GameHub> hubContext, ILoggerFactory loggerFactory) {
			_lobbyService = lobbyService;
			_userManager = userManager;
			_gameService = gameService;
			_soloQueueService = soloQueueService;
			_logger = loggerFactory.CreateLogger<GameHub>();
		}

		//TESTED
		public void CreateLobby(string statusCode) {
			LobbyStatus status = Enum.Parse<LobbyStatus>(statusCode);
			if (_lobbyService.FindUserLobby(Context.UserIdentifier) != null) {
				Clients.Caller.CannotCreateLobbyFromOtherLobby();
				return;
			}

			LobbyModel lobby = _lobbyService.CreateLobby(Context.UserIdentifier, status);
			_logger.LogInformation($"Lobby created by {Context.UserIdentifier}. With Id: {lobby.Data.LobbyId}");

			Clients.Caller.LobbyCreated(lobby.Data);
			Clients.All.LobbyAddedHandler(lobby.Data);
		}

		public async Task CreateMatchAsync(int lobbyId) {
			try {
				LobbyModel lobby = _lobbyService.FindLobbyById(lobbyId);
				ApplicationUser player1 = await _userManager.FindByNameAsync(lobby.Data.Host);
				ApplicationUser player2 = await _userManager.FindByNameAsync(lobby.Data.Guest);
				Match newMatch = _gameService.CreateMatch(player1, player2, lobby.Data.BoardHeight, lobby.Data.BoardWidth);
				_logger.LogInformation($"New match created from lobby: {lobbyId}");
				_lobbyService.DeleteLobby(lobbyId);
				_logger.LogInformation($"Lobby with Id: {lobbyId} was removed.");
				MatchDto dto = new MatchDto {
					MatchId = newMatch.MatchId,
					OtherPlayer = lobby.Data.Guest,
					BoardData = newMatch.BoardData,
					State = "YourTurn"
				};

				await Clients.Caller.MatchCreated(dto);
				dto.OtherPlayer = lobby.Data.Host;
				dto.State = "EnemyTurn";
				await Clients.User(lobby.Data.Guest).MatchCreated(dto);
				await Clients.All.LobbyDeleted(lobbyId);
			} catch (ArgumentException) {
				await Clients.Caller.NotEnoughPlayersHandler();
			}
		}

		public void LobbySettingsChanged(LobbyData data) {
			LobbyModel lobby = _lobbyService.FindLobbyById(data.LobbyId);
			if (Context.User.Identity.Name != lobby.Data.Host) {
				Clients.Caller.CannotSetOtherLobby();
				return;
			}

			lobby.Data.BoardHeight = data.BoardHeight;
			lobby.Data.BoardWidth = data.BoardWidth;
			lobby.Data.Status = data.Status;
			_logger.LogInformation($"The settings of lobby #{data.LobbyId} were updated.");

			if (lobby.Data.Guest != null) {
				Clients.User(lobby.Data.Guest).LobbySettingsChanged(lobby.Data);
			}
		}

		public void SendInvitationTo(int lobbyId, string user) {
			_lobbyService.InvitePlayerToLobby(lobbyId, user);
			_logger.LogInformation($"{user} was invited to lobby #{lobbyId} by {Context.UserIdentifier}.");
			Clients.User(user).GetInvitationTo(lobbyId);
		}

		//TESTED
		public void JoinLobby(int lobbyId) {
			LobbyModel model = _lobbyService.FindUserLobby(Context.UserIdentifier);
			if (model != null) {
				string originalHost = model.Data.Host;
				_lobbyService.DisconnectPlayerFromLobby(Context.UserIdentifier, model.Data.LobbyId);
				_logger.LogInformation($"{Context.UserIdentifier} has disconnected from lobby #{model.Data.LobbyId}");

				if (model.Data.Host != null) {
					if (model.Data.Host == originalHost) {
						Clients.User(model.Data.Host).GuestDisconnected();
					} else {
						Clients.User(model.Data.Host).HostDisconnected();
					}
				} else {
					Clients.All.LobbyDeleted(lobbyId);
				}
			}

			if (_lobbyService.JoinPlayerToLobby(Context.UserIdentifier, lobbyId)) {
				LobbyModel lobbyModel = _lobbyService.FindLobbyById(lobbyId);
				_logger.LogInformation($"{Context.UserIdentifier} joined lobby #{lobbyId}");

				Clients.Caller.JoinedToLobby(lobbyModel.Data);
				Clients.User(lobbyModel.Data.Host).PlayerJoinedToLobby(Context.UserIdentifier);
			} else {
				Clients.Caller.FailedToJoinLobby();
			}
		}

		public List<MatchDto> GetMatches() {
			return _gameService.GetMatchesOf(Context.UserIdentifier);
		}

		public void PlaceItem(int matchId, int column) {
			Match match = _gameService.GetMatchById(matchId);
			if (match == null) {
				Clients.Caller.IncorrectMatchIdHandler();
				return;
			}

			string otherPlayer = _gameService.GetOtherPlayer(matchId, Context.UserIdentifier).UserName;
			switch (_gameService.PlaceItemToColumn(matchId, column, Context.UserIdentifier)) {
				case PlacementResult.ColumnFull:
					Clients.Caller.ColumnFullHandler();
					break;
				case PlacementResult.MatchNotRunning:
					Clients.Caller.MatchFinishedHandler();
					break;
				case PlacementResult.NotYourTurn:
					Clients.Caller.NotEnoughPlayersHandler();
					break;
				case PlacementResult.Success:
					_logger.LogInformation($"{Context.UserIdentifier} placed an item at column #{column} in match #{matchId}");
					Clients.Caller.SuccessfulPlacement(column);
					Clients.User(otherPlayer).SuccessfulEnemyPlacement(column);
					break;
				case PlacementResult.Victory:
					_logger.LogInformation($"{Context.UserIdentifier} placed an item at column #{column} in match #{matchId}");
					_logger.LogInformation($"{Context.UserIdentifier} won match #{matchId}");
					Clients.Caller.VictoryHandler(column);
					Clients.User(otherPlayer).EnemyVictoryHandler(column);
					break;
			}
		}

		public async Task JoinSoloQueAsync() {
			_soloQueueService.QueingPlayers.Add(Context.UserIdentifier);
			_logger.LogInformation($"{Context.UserIdentifier} joined the solo queue.");

			if (_soloQueueService.QueingPlayers.Count >= 2) {
				ApplicationUser player1 = await _userManager.FindByNameAsync(_soloQueueService.QueingPlayers[0]);
				ApplicationUser player2 = await _userManager.FindByNameAsync(_soloQueueService.QueingPlayers[1]);

				Match match = _gameService.CreateMatch(player1, player2, 6, 7);
				_soloQueueService.QueingPlayers.Remove(player1.UserName);
				_soloQueueService.QueingPlayers.Remove(player2.UserName);
				_logger.LogInformation($"Match created from solo queue with {player1.UserName} and {player2.UserName}.");

				MatchDto dto = new MatchDto {
					MatchId = match.MatchId,
					OtherPlayer = player2.UserName,
					BoardData = match.BoardData,
					State = "YourTurn"
				};

				await Clients.User(player1.UserName).MatchCreated(dto);
				dto.State = "EnemyTurn";
				dto.OtherPlayer = player1.UserName;
				await Clients.User(player2.UserName).MatchCreated(dto);
			}
		}

		public void LeaveSoloQue() {
			_soloQueueService.QueingPlayers.Remove(Context.UserIdentifier);
			_logger.LogInformation($"{Context.UserIdentifier} left the solo queue.");
		}

		public void KickGuest(int lobbyId) {
			LobbyModel model = _lobbyService.FindLobbyById(lobbyId);

			if (Context.UserIdentifier != model.Data.Host) {
				Clients.Caller.CannotSetOtherLobby();
				return;
			}

			string kickedGuest = _lobbyService.KickGuest(lobbyId);
			_logger.LogInformation($"{kickedGuest} was kicked from lobby #{lobbyId}.");
			Clients.Caller.GuestKicked();
			Clients.User(kickedGuest).YouHaveBeenKicked();
		}

		public List<LobbyData> GetLobbies() {
			return _lobbyService.GetLobbyData();
		}

		//TESTED
		public void DisconnectFromLobby(int lobbyId) {
			LobbyModel lobby = _lobbyService.FindLobbyById(lobbyId);
			string originalHost = lobby.Data.Host;
			_lobbyService.DisconnectPlayerFromLobby(Context.UserIdentifier, lobbyId);
			_logger.LogInformation($"{Context.UserIdentifier} has disconnected from lobby #{lobbyId}");

			if (lobby.Data.Host != null) {
				if (lobby.Data.Host == originalHost) {
					Clients.User(lobby.Data.Host).GuestDisconnected();
				} else {
					Clients.User(lobby.Data.Host).HostDisconnected();
				}
			} else {
				Clients.All.LobbyDeleted(lobby.Data.LobbyId);
			}
		}

		public override Task OnDisconnectedAsync(Exception exception) {
			LobbyModel model = _lobbyService.FindUserLobby(Context.UserIdentifier);
			if (model != null) {
				_lobbyService.DisconnectPlayerFromLobby(Context.UserIdentifier, model.Data.LobbyId);
			}
			return base.OnDisconnectedAsync(exception);
		}
	}
}
