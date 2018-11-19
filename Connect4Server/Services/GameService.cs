using Connect4Server.Data;
using Connect4Server.Models.Board;
using Connect4Server.Models.Lobby;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Connect4Dtos;
using Microsoft.EntityFrameworkCore;
using Match = Connect4Server.Data.Match;

namespace Connect4Server.Services {
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
				State = GameState.Player1Moves,
				BoardData = new Board(width, height).ToString()
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
				MatchDto dto = new MatchDto {
					MatchId = m.MatchId,
					OtherPlayer = m.Player1.UserName == user ? m.Player2.UserName : m.Player1.UserName,
					BoardData = m.BoardData,
					State = m.State,
					YourItem = m.Player1.UserName == user ? Item.Red : Item.Yellow
				};

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
			return _context.Matches.SingleOrDefault(m => m.MatchId == id);
		}

		public PlacementResult PlaceItemToColumn(int matchId, int column, string player) {
			Match match = GetMatchById(matchId);
			bool isPlayerOne = player == match.Player1.UserName;

			if (match.State == GameState.Player1Won || match.State == GameState.Player2Won) {
				return PlacementResult.MatchNotRunning;
			}

			if (isPlayerOne && match.State == GameState.Player2Moves ||
			    !isPlayerOne && match.State == GameState.Player1Moves) {
				return PlacementResult.NotYourTurn;
			}

			Board board = Board.Parse(match.BoardData);
			Item item = isPlayerOne ? Item.Red : Item.Yellow;
			if (board.PutItemToColumn(item, column)) {
				match.BoardData = board.ToString();

				if (board.CheckWinner() == item) {
					match.State = isPlayerOne ? GameState.Player1Won : GameState.Player2Won;

					_context.SaveChanges();
					return PlacementResult.Victory;
				}

				match.State = isPlayerOne ? GameState.Player2Moves : GameState.Player1Moves;
				_context.SaveChanges();
				return PlacementResult.Success;
			}

			return PlacementResult.ColumnFull;
		}
	}
}
