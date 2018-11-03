using Connect4Server.Data;
using Connect4Server.Models.Lobby;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4Server.Services {
	public class LobbyService {
		private Dictionary<int, LobbyModel> lobbies;

		public ReadOnlyDictionary<int, LobbyModel> Lobbies {
			get {
				return new ReadOnlyDictionary<int, LobbyModel>(lobbies);
			}
		}

		public LobbyService() {
			lobbies = new Dictionary<int, LobbyModel>();
		}

		/// <summary>
		/// Creates a new lobby instance and returns its id
		/// </summary>
		/// <param name="host">The player that created the lobby</param>
		/// <param name="status">Status whether the lobby is private or public</param>
		/// <returns></returns>
		public int CreateLobby(ApplicationUser host, LobbyStatus status) {
			int newLobbyId = 0;
			while (lobbies.ContainsKey(newLobbyId)) {
				newLobbyId++;
			}

			LobbyModel model = new LobbyModel(host);
			model.Settings.Status = status;

			lobbies.Add(newLobbyId, model);

			return newLobbyId;
		}

		public bool JoinPlayerToLobby(ApplicationUser player, int lobbyId) {
			return lobbies[lobbyId].JoinGuest(player);
		}

		public void DisconnectPlayerFromLobby(ApplicationUser player, int lobbyId) {
			if (lobbies[lobbyId].Guest == null) {
				lobbies.Remove(lobbyId);
				return;
			}

			lobbies[lobbyId].DisconnectPlayer(player);
		}
	}
}
