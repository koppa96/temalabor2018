using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4Server.Models.Board {
	public enum PlacementResult {
		ColumnFull, NotYourTurn, Success, Victory, MatchNotRunning
	}
}
