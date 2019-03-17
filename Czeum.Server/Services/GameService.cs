using Czeum.Server.Models.Lobby;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Czeum.DAL;
using Czeum.Entities;
using Connect4Dtos;
using Czeum.Server.Models.Board;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Server.Services {
	public class GameService {
		private readonly ApplicationDbContext _context;

		public GameService(ApplicationDbContext context) {
			_context = context;
		}

		public Match CreateMatch(ApplicationUser player1, ApplicationUser player2, int height, int width) {
			if (player1 == null || player2 == null) {
				throw new ArgumentException("Two players are needed for a game");
			}

			Match match = new Match {
				Player1 = player1,
				Player2 = player2,
				State = MatchState.Player1Moves,
                //TODO Change match creation
				Board = new SerializedConnect4Board() //new Board(width, height).ToString()
			};

			_context.Matches.Add(match);
			_context.SaveChanges();

			return match;
		}

		public List<MatchDto> GetMatchesOf(string user) {
			var qMatches = from m in _context.Matches.Include("Player1").Include("Player2")
						   where m.Player1.UserName == user || m.Player2.UserName == user
						   select m;

			List<MatchDto> dtos = new List<MatchDto>();

			foreach (Match m in qMatches) {
				bool isPlayer1 = m.Player1.UserName == user;
				bool isPlayer2 = m.Player2.UserName == user;

				MatchDto dto = new MatchDto {
					MatchId = m.MatchId,
					OtherPlayer = isPlayer1 ? m.Player2.UserName : m.Player1.UserName,
					BoardData = m.Board.BoardData,
					YourItem = isPlayer1 ? Item.Red : Item.Yellow
				};

				if (!isPlayer1 && !isPlayer2) {
					dto.State = GameState.NotYourMatch;
				} else {
					switch (m.State) {
						case MatchState.Player1Moves:
							dto.State = isPlayer1 ? GameState.YourTurn : GameState.EnemyTurn;
							break;
						case MatchState.Player2Moves:
							dto.State = isPlayer2 ? GameState.YourTurn : GameState.EnemyTurn;
							break;
						case MatchState.Player1Won:
							dto.State = isPlayer1 ? GameState.YouWon : GameState.EnemyWon;
							break;
						case MatchState.Player2Won:
							dto.State = isPlayer2 ? GameState.YouWon : GameState.EnemyWon;
							break;
						case MatchState.Draw:
							dto.State = GameState.Draw;
							break;
					}
				}

				dtos.Add(dto);
			}

			return dtos;
		}

		public ApplicationUser GetOtherPlayer(int matchId, string player) {
			Match match = GetMatchById(matchId);

			if (match == null) {
				throw new ArgumentException("Invalid match id.");
			}

			return match.Player1.UserName == player ? match.Player2 : match.Player1;
		}

		public Match GetMatchById(int id) {
			return _context.Matches.Include("Player1")
								   .Include("Player2")
								   .SingleOrDefault(m => m.MatchId == id);
		}

		public MatchDto GetMatchDtoFor(int id, string user) {
			Match match = GetMatchById(id);

			if (match == null) {
				throw new ArgumentException("Invalid match identifier");
			}

			bool isPlayer1 = match.Player1.UserName == user;
			bool isPlayer2 = match.Player2.UserName == user;

			MatchDto dto = new MatchDto {
				MatchId = match.MatchId,
				OtherPlayer = isPlayer1 ? match.Player2.UserName : match.Player1.UserName,
				BoardData = match.Board.BoardData,
				YourItem = isPlayer1 ? Item.Red : Item.Yellow
			};

			if (!isPlayer1 && !isPlayer2) {
				dto.State = GameState.NotYourMatch;
			} else {
				switch (match.State) {
					case MatchState.Player1Moves:
						dto.State = isPlayer1 ? GameState.YourTurn : GameState.EnemyTurn;
						break;
					case MatchState.Player2Moves:
						dto.State = isPlayer2 ? GameState.YourTurn : GameState.EnemyTurn;
						break;
					case MatchState.Player1Won:
						dto.State = isPlayer1 ? GameState.YouWon : GameState.EnemyWon;
						break;
					case MatchState.Player2Won:
						dto.State = isPlayer2 ? GameState.YouWon : GameState.EnemyWon;
						break;
					case MatchState.Draw:
						dto.State = GameState.Draw;
						break;
				}
			}

			return dto;
		}

		public PlacementResult PlaceItemToColumn(int matchId, int column, string player) {
			Match match = GetMatchById(matchId);
			bool isPlayerOne = player == match.Player1.UserName;

			if (match.State == MatchState.Player1Won || match.State == MatchState.Player2Won) {
				return PlacementResult.MatchNotRunning;
			}

			if (isPlayerOne && match.State == MatchState.Player2Moves ||
			    !isPlayerOne && match.State == MatchState.Player1Moves) {
				return PlacementResult.NotYourTurn;
			}

			Board board = Board.Parse(match.Board.BoardData);
			Item item = isPlayerOne ? Item.Red : Item.Yellow;
			if (board.PutItemToColumn(item, column)) {
				match.Board.BoardData = board.ToString();

				if (board.CheckWinner() == item) {
					match.State = isPlayerOne ? MatchState.Player1Won : MatchState.Player2Won;

					_context.SaveChanges();
					return PlacementResult.Victory;
				}

				if (board.Full) {
					match.State = MatchState.Draw;

					_context.SaveChanges();
					return PlacementResult.Draw;
				}

				match.State = isPlayerOne ? MatchState.Player2Moves : MatchState.Player1Moves;
				_context.SaveChanges();
				return PlacementResult.Success;
			}

			return PlacementResult.ColumnFull;
		}
	}
}
