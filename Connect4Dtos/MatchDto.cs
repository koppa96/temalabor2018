using System;

namespace Connect4Dtos {
	public class MatchDto {
		public int MatchId { get; set; }
		public string OtherPlayer { get; set; }
		public string BoardData { get; set; }
		public string State { get; set; }
	}
}
