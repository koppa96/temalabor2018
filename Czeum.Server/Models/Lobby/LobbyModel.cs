using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Czeum.DTO.Lobby;

namespace Czeum.Server.Models.Lobby {
	public class LobbyModel {
		public LobbyData Data { get; }

		public LobbyModel(int id, string host, LobbyAccess access) {
			Data = new LobbyData {
				LobbyId = id,
				Host = host,
				InvitedPlayers = new List<string>(),
				BoardHeight = 6,
				BoardWidth = 7,
				Access = access
			};
		}

		public bool JoinGuest(string player) {
			if (Data.Guest == null && (Data.Access == LobbyAccess.Public || Data.InvitedPlayers.Contains(player))) {
				Data.InvitedPlayers.Remove(player);
				Data.Guest = player;
				return true;
			}

			return false;
		}

		public void DisconnectPlayer(string player) {
			if (Data.Host == player) {
				Data.Host = Data.Guest;
				Data.Guest = null;
			} else if (Data.Guest == player) {
				Data.Guest = null;
			} else {
				throw new ArgumentException("The player is not joined to the lobby");
			}
		}
	}
}
