using Connect4Server.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Connect4Dtos;

namespace Connect4Server.Models.Lobby {
	public class LobbyModel {
		public LobbyData Data { get; }

		public LobbyModel(int id, string host, LobbyStatus status) {
			Data = new LobbyData {
				LobbyId = id,
				Host = host,
				InvitedPlayers = new List<string>(),
				BoardHeight = 6,
				BoardWidth = 7,
				Status = status
			};
		}

		public bool JoinGuest(string player) {
			if (Data.Guest == null && (Data.Status == LobbyStatus.Public || Data.InvitedPlayers.Contains(player))) {
				Data.Guest = player;
				return true;
			}

			return false;
		}

		public void DisconnectPlayer(string player) {
			if (Data.Host == player) {
				Data.Host = Data.Guest;
			} else if (Data.Guest == player) {
				Data.Guest = null;
			} else {
				throw new ArgumentException("The player is not joined to the lobby");
			}
		}
	}
}
