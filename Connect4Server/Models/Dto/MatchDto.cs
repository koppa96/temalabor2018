using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4Server.Models.Dto {
	public class MatchDto {
		public int MatchId { get; set; }
		public string OtherPlayer { get; set; }
		public string BoardData { get; set; }
		public string State { get; set; }
	}
}
