using Connect4Server.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4Server.Models.Lobby {
	public class LobbyModel {
		public ApplicationUser Host { get; private set; }
		public ApplicationUser Guest { get; private set; }
		public LobbySettings Settings { get; set; }
		public List<ApplicationUser> InvitedPlayers { get; set; }

		public LobbyModel(ApplicationUser host) {
			Host = host;
			InvitedPlayers = new List<ApplicationUser>();
			Settings = new LobbySettings() {
				BoardHeight = 6,
				BoardWidth = 7,
				Status = LobbyStatus.Public
			};
		}

		public bool JoinGuest(ApplicationUser player) {
			if (Guest == null && (Settings.Status == LobbyStatus.Public || InvitedPlayers.Contains(player))) {
				Guest = player;
				return true;
			}

			return false;
		}

		public void DisconnectPlayer(ApplicationUser player) {
			if (Host.Id == player.Id) {
				Host = Guest;
			} else if (Guest.Id == player.Id) {
				Guest = null;
			} else {
				throw new ArgumentException("The player is not joined to the lobby");
			}
		}
	}
}
