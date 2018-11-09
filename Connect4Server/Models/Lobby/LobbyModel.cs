using Connect4Server.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4Server.Models.Lobby {
	public class LobbyModel {
		public string Host { get; private set; }
		public string Guest { get; private set; }
		public int BoardHeight { get; set; }
		public int BoardWidth { get; set; }
		public LobbyStatus Status { get; set; }
		public List<string> InvitedPlayers { get; set; }

		public LobbyModel(string host, LobbyStatus status) {
			Host = host;
			InvitedPlayers = new List<string>();
			BoardHeight = 6;
			BoardWidth = 7;
			Status = status;
		}

		public bool JoinGuest(string player) {
			if (Guest == null && (Status == LobbyStatus.Public || InvitedPlayers.Contains(player))) {
				Guest = player;
				return true;
			}

			return false;
		}

		public void DisconnectPlayer(string player) {
			if (Host == player) {
				Host = Guest;
			} else if (Guest == player) {
				Guest = null;
			} else {
				throw new ArgumentException("The player is not joined to the lobby");
			}
		}
	}
}
