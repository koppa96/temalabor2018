using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4Server.Models.Lobby {
	public class LobbySettings {
		public int BoardHeight { get; set; }
		public int BoardWidth { get; set; }
		public LobbyStatus Status { get; set; }
	}
}
