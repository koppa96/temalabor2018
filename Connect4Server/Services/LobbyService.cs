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
		public int CreateLobby(string host, LobbyStatus status) {
			int newLobbyId = 0;
			while (lobbies.ContainsKey(newLobbyId)) {
				newLobbyId++;
			}

			LobbyModel model = new LobbyModel(host, status);
			lobbies.Add(newLobbyId, model);

			return newLobbyId;
		}

		public bool JoinPlayerToLobby(string player, int lobbyId) {
			return lobbies[lobbyId].JoinGuest(player);
		}

		public void DisconnectPlayerFromLobby(string player, int lobbyId) {
			lobbies[lobbyId].DisconnectPlayer(player);

			if (lobbies[lobbyId].Host == null) {
				lobbies.Remove(lobbyId);
			}			
		}

		public void DeleteLobby(int lobbyId) {
			lobbies.Remove(lobbyId);
		}

		public void InvitePlayerToLobby(int lobbyId, string player) {
			lobbies[lobbyId].InvitedPlayers.Add(player);
		}

		public string KickGuest(int lobbyId) {
			string guestName = lobbies[lobbyId].Guest;
			if (lobbies[lobbyId].Guest != null) {
				lobbies[lobbyId].DisconnectPlayer(lobbies[lobbyId].Guest);
			}

			return guestName;
		}
	}
}
