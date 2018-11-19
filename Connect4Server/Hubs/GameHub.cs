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
						SoloQueueService soloQueueService, ILoggerFactory loggerFactory) {
			_lobbyService = lobbyService;
			_userManager = userManager;
			_gameService = gameService;
			_soloQueueService = soloQueueService;
			_logger = loggerFactory.CreateLogger<GameHub>();
		}

		/// <summary>
		/// Creates a new lobby instance.
		/// Notifies the caller that the lobby is ready.
		/// Notifies everyone to update their lobby list.
		/// </summary>
		/// <param name="statusCode">The visibility of the lobby</param>
		public void CreateLobby(string statusCode) {
			LobbyStatus status = Enum.Parse<LobbyStatus>(statusCode);
			if (_lobbyService.FindUserLobby(Context.UserIdentifier) != null) {
				Clients.Caller.CannotCreateLobbyFromOtherLobby();
				return;
			}

			if (_soloQueueService.IsQueuing(Context.UserIdentifier)) {
				Clients.Caller.CannotJoinOrCreateWhileQueuing();
				return;
			}

			LobbyModel lobby = _lobbyService.CreateLobby(Context.UserIdentifier, status);
			_logger.LogInformation($"Lobby created by {Context.UserIdentifier}. With Id: {lobby.Data.LobbyId}");

			Clients.Caller.LobbyCreated(lobby.Data);
			Clients.All.LobbyAddedHandler(lobby.Data);
		}

		/// <summary>
		/// Creates a match from the given lobby.
		/// Notifies the players when the match is ready.
		/// Notifies host if not enough players.
		/// Notifies everyone to update their lobby list.
		/// </summary>
		/// <param name="lobbyId">The id of the lobby</param>
		/// <returns></returns>
		public async Task CreateMatchAsync(int lobbyId) {
			try {
				LobbyModel lobby = _lobbyService.FindLobbyById(lobbyId);
				if (lobby.Data.Host != Context.UserIdentifier) {
					await Clients.Caller.CannotStartOtherMatch();
					return;
				}

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

		/// <summary>
		/// Changes the settings of the lobby to the desired settings given as parameter.
		/// Notifies everyone to update their lobby list.
		/// </summary>
		/// <param name="data">The desired settings</param>
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

			Clients.All.LobbyChanged(lobby.Data);
		}

		/// <summary>
		/// Adds the user with the given username to the invitation list.
		/// Notifies the caller that the user has been invited.
		/// Notifies the user that they have been invited.
		/// </summary>
		/// <param name="lobbyId">The id of the lobby that the user is invited to</param>
		/// <param name="user">The name of the invitee</param>
		public void SendInvitationTo(int lobbyId, string user) {
			LobbyModel lobby = _lobbyService.FindLobbyById(lobbyId);
			if (lobby == null) {
				Clients.Caller.InvalidLobbyId();
				return;
			}

			if (Context.UserIdentifier != lobby.Data.Host) {
				Clients.Caller.OnlyHostCanInvite();
				return;
			}

			_lobbyService.InvitePlayerToLobby(lobbyId, user);
			_logger.LogInformation($"{user} was invited to lobby #{lobbyId} by {Context.UserIdentifier}.");
			Clients.Caller.UserInvited(_lobbyService.FindLobbyById(lobbyId).Data);
			Clients.User(user).GetInvitationTo(lobbyId);
			Clients.All.LobbyChanged(lobby.Data);
		}

		public void CancelInvitationOf(int lobbyId, string user) {
			LobbyModel lobby = _lobbyService.FindLobbyById(lobbyId);
			if (lobby == null) {
				Clients.Caller.InvalidLobbyId();
				return;
			}

			if (Context.UserIdentifier != lobby.Data.Host) {
				Clients.Caller.OnlyHostCanInvite();
				return;
			}

			lobby.Data.InvitedPlayers.Remove(user);
			Clients.All.LobbyChanged(lobby.Data);
		}

		/// <summary>
		/// Joins the player to the desired lobby if it is possible. If the player is already in a lobby they will be disconnected from there.
		/// </summary>
		/// <param name="lobbyId">The id of the desired lobby</param>
		public void JoinLobby(int lobbyId) {
			LobbyModel model = _lobbyService.FindUserLobby(Context.UserIdentifier);

			if (_soloQueueService.IsQueuing(Context.UserIdentifier)) {
				Clients.Caller.CannotJoinOrCreateWhileQueuing();
				return;
			}

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

					Clients.All.LobbyChanged(model.Data);
				} else {
					Clients.All.LobbyDeleted(lobbyId);
				}
			}

			if (_lobbyService.JoinPlayerToLobby(Context.UserIdentifier, lobbyId)) {
				LobbyModel lobbyModel = _lobbyService.FindLobbyById(lobbyId);
				_logger.LogInformation($"{Context.UserIdentifier} joined lobby #{lobbyId}");

				Clients.Caller.JoinedToLobby(lobbyModel.Data);
				Clients.User(lobbyModel.Data.Host).PlayerJoinedToLobby(Context.UserIdentifier);
				Clients.All.LobbyChanged(lobbyModel.Data);
			} else {
				Clients.Caller.FailedToJoinLobby();
			}
		}

		/// <summary>
		/// Gets the list of matches that the caller has played or is playing at the moment.
		/// </summary>
		/// <returns>The list of matches</returns>
		public List<MatchDto> GetMatches() {
			return _gameService.GetMatchesOf(Context.UserIdentifier);
		}

		/// <summary>
		/// Places an item in the selected match, in the selected column.
		/// Notifies the caller and the other player from the result.
		/// </summary>
		/// <param name="matchId">The identifier of the match</param>
		/// <param name="column">The index of the column</param>
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
					Clients.Caller.NotYourTurnHandler();
					break;
				case PlacementResult.Success:
					_logger.LogInformation($"{Context.UserIdentifier} placed an item at column #{column} in match #{matchId}");
					Clients.Caller.SuccessfulPlacement(matchId, column);
					Clients.User(otherPlayer).SuccessfulEnemyPlacement(matchId, column);
					break;
				case PlacementResult.Victory:
					_logger.LogInformation($"{Context.UserIdentifier} placed an item at column #{column} in match #{matchId}");
					_logger.LogInformation($"{Context.UserIdentifier} won match #{matchId}");
					Clients.Caller.VictoryHandler(matchId, column);
					Clients.User(otherPlayer).EnemyVictoryHandler(matchId, column);
					break;
			}
		}

		/// <summary>
		/// Joins the caller to the solo queue. If there are more than 1 players on the list it creates a match with the first two.
		/// </summary>
		/// <returns></returns>
		public async Task JoinSoloQueueAsync() {
			_soloQueueService.JoinSoloQueue(Context.UserIdentifier);
			_logger.LogInformation($"{Context.UserIdentifier} joined the solo queue.");

			if (_lobbyService.FindUserLobby(Context.UserIdentifier) != null) {
				await Clients.Caller.CannotQueueWhileInLobby();
				return;
			}

			string[] players = _soloQueueService.PopFirstTwoPlayers();
			if (players != null) { 
				ApplicationUser player1 = await _userManager.FindByNameAsync(players[0]);
				ApplicationUser player2 = await _userManager.FindByNameAsync(players[1]);

				Match match = _gameService.CreateMatch(player1, player2, 6, 7);
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

		/// <summary>
		/// The caller leaves the solo queue
		/// </summary>
		public void LeaveSoloQueue() {
			_soloQueueService.LeaveSoloQueue(Context.UserIdentifier);
			_logger.LogInformation($"{Context.UserIdentifier} left the solo queue.");
		}

		/// <summary>
		/// Removes the guest of the given lobby
		/// </summary>
		/// <param name="lobbyId">The identifier of the lobby</param>
		public void KickGuest(int lobbyId) {
			LobbyModel model = _lobbyService.FindLobbyById(lobbyId);

			if (Context.UserIdentifier != model.Data.Host) {
				Clients.Caller.CannotSetOtherLobby();
				return;
			}

			if (string.IsNullOrEmpty(model.Data.Guest)) {
				Clients.Caller.NobodyToKick();
			}

			string kickedGuest = _lobbyService.KickGuest(lobbyId);
			_logger.LogInformation($"{kickedGuest} was kicked from lobby #{lobbyId}.");
			Clients.Caller.GuestKicked();
			Clients.User(kickedGuest).YouHaveBeenKicked();
			Clients.All.LobbyChanged(model.Data);
		}

		/// <summary>
		/// Gets a list of the data of the currently existing lobbies
		/// </summary>
		/// <returns></returns>
		public List<LobbyData> GetLobbies() {
			return _lobbyService.GetLobbyData();
		}

		/// <summary>
		/// Disconnects the caller from the given lobby.
		/// </summary>
		/// <param name="lobbyId">The identifier of the lobby</param>
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

				Clients.All.LobbyChanged(lobby.Data);
			} else {
				Clients.All.LobbyDeleted(lobby.Data.LobbyId);
			}
		}

		/// <summary>
		/// Called when someone disconnects from the hub. If they are in a lobby, they will be disconnected from there.
		/// </summary>
		/// <param name="exception"></param>
		/// <returns></returns>
		public override Task OnDisconnectedAsync(Exception exception) {
			LobbyModel lobby = _lobbyService.FindUserLobby(Context.UserIdentifier);

			if (lobby != null) {
				string originalHost = lobby.Data.Host;
				_lobbyService.DisconnectPlayerFromLobby(Context.UserIdentifier, lobby.Data.LobbyId);

				if (lobby.Data.Host == null) {
					Clients.All.LobbyDeleted(lobby.Data.LobbyId);
				} else {
					if (lobby.Data.Host == originalHost) {
						Clients.User(lobby.Data.Host).GuestDisconnected();
					} else {
						Clients.User(lobby.Data.Host).HostDisconnected();
					}

					Clients.All.LobbyChanged(lobby.Data);
				}
			}
			return base.OnDisconnectedAsync(exception);
		}
	}
}
