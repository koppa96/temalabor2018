using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czeum.Server.Models.Board {
	public enum PlacementResult {
		ColumnFull, NotYourTurn, Success, Victory, MatchNotRunning, Draw
	}
}
