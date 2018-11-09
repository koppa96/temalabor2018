using Connect4Server.Data;
using Connect4Server.Models.Board;
using Connect4Server.Models.Lobby;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

		public List<Match> GetMatchesOf(string user) {
			var qMatches = from m in context.Matches
						   where m.Player1.UserName == user || m.Player2.UserName == user
						   select m;

			return qMatches.ToList();
		}

		public Match GetMatchById(int id) {
			return context.Matches.Where(m => m.MatchId == id)
								  .SingleOrDefault();
		}
	}
}
