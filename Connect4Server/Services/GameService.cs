using Connect4Server.Data;
using Connect4Server.Models.Board;
using Connect4Server.Models.Lobby;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Connect4Server.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Match = Connect4Server.Data.Match;

namespace Connect4Server.Services {
	public class GameService {
		ApplicationDbContext context;

		public GameService(ApplicationDbContext context) {
			this.context = context;
		}

		public Match CreateMatch(ApplicationUser player1, ApplicationUser player2, int height, int width) {
			if (player1 == null || player2 == null) {
				throw new ArgumentException("Two players are needed for a game");
			}

			Match match = new Match() {
				Player1 = player1,
				Player2 = player2,
				State = "Player1Moves",
				BoardData = new Board(width, height).ToString()
			};

			context.Matches.Add(match);
			context.SaveChanges();

			return match;
		}

		public List<MatchDto> GetMatchesOf(string user) {
			var qMatches = from m in context.Matches
						   where m.Player1.UserName == user || m.Player2.UserName == user
						   select m;

			List<MatchDto> dtos = new List<MatchDto>();
			foreach (Match m in qMatches) {
				MatchDto dto = new MatchDto {
					MatchId = m.MatchId,
					OtherPlayer = m.Player1.UserName == user ? m.Player2.UserName : m.Player1.UserName,
					BoardData = m.BoardData
				};

				switch (m.State) {
					case "Player1Moves":
						dto.State = user == m.Player1.UserName ? "YourTurn" : "EnemyTurn";
						break;
					case "Player2Moves":
						dto.State = user == m.Player2.UserName ? "YourTurn" : "EnemyTurn";
						break;
					case "Player1Won":
						dto.State = user == m.Player1.UserName ? "YouWon" : "YouLost";
						break;
					case "Player2Won":
						dto.State = user == m.Player2.UserName ? "YouWon" : "YouLost";
						break;
					default:
						dto.State = "Unavailable";
						break;
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
			return context.Matches.SingleOrDefault(m => m.MatchId == id);
		}

		public PlacementResult PlaceItemToColumn(int matchId, int column, string player) {
			Match match = GetMatchById(matchId);
			bool isPlayerOne = player == match.Player1.UserName;

			if (match.State == "Player1Won" || match.State == "Player2Won") {
				return PlacementResult.MatchNotRunning;
			}

			if (isPlayerOne && match.State == "Player2Moves" ||
			    !isPlayerOne && match.State == "Player1Moves") {
				return PlacementResult.NotYourTurn;
			}

			Board board = Board.Parse(match.BoardData);
			Item item = isPlayerOne ? Item.Red : Item.Yellow;
			if (board.PutItemToColumn(item, column)) {
				match.BoardData = board.ToString();

				if (board.CheckWinner() == item) {
					match.State = isPlayerOne ? "Player1Won" : "Player2Won";

					context.SaveChanges();
					return PlacementResult.Victory;
				}

				match.State = isPlayerOne ? "Player2Moves" : "Player1Moves";
				context.SaveChanges();
				return PlacementResult.Success;
			}

			return PlacementResult.ColumnFull;
		}

		public string GetBoardOfMatch(int matchId) {
			Match match = GetMatchById(matchId);
			return match.BoardData;
		}
	}
}
