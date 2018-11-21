using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4Server.Models.Board {
	public enum MatchState {
		Player1Moves, Player2Moves, Player1Won, Player2Won, Draw
	}
}
