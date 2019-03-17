using Czeum.Server.Models.Lobby;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Connect4Dtos;
using LobbyModel = Czeum.Server.Models.Lobby.LobbyModel;

namespace Czeum.Server.Services {
	public class LobbyService {
		public List<Models.Lobby.LobbyModel> Lobbies { get; }

		public LobbyService() {
			Lobbies = new List<Models.Lobby.LobbyModel>();
		}

		/// <summary>
		/// Creates a new lobby instance and returns its id
		/// </summary>
		/// <param name="host">The player that created the lobby</param>
		/// <param name="status">Status whether the lobby is private or public</param>
		/// <returns></returns>
		public Models.Lobby.LobbyModel CreateLobby(string host, LobbyStatus status) {
			int newLobbyId = 0;
			while (Lobbies.SingleOrDefault(l => l.Data.LobbyId == newLobbyId) != null) {
				newLobbyId++;
			}

			Models.Lobby.LobbyModel model = new Models.Lobby.LobbyModel(newLobbyId, host, status);
			Lobbies.Add(model);

			return model;
		}

		public bool JoinPlayerToLobby(string player, int lobbyId) {
			Models.Lobby.LobbyModel lobby = FindLobbyById(lobbyId);
			if (lobby == null) {
				throw new ArgumentException("Invalid lobby id");
			}

			return lobby.JoinGuest(player);
		}

		public void DisconnectPlayerFromLobby(string player, int lobbyId) {
			Models.Lobby.LobbyModel lobby = FindLobbyById(lobbyId);
			lobby.DisconnectPlayer(player);

			if (lobby.Data.Host == null) {
				Lobbies.Remove(lobby);
			}			
		}

		public void DeleteLobby(int lobbyId) {
			Lobbies.Remove(FindLobbyById(lobbyId));
		}

		public void InvitePlayerToLobby(int lobbyId, string player) {
			FindLobbyById(lobbyId)?.Data.InvitedPlayers.Add(player);
		}

		public string KickGuest(int lobbyId) {
			Models.Lobby.LobbyModel lobby = FindLobbyById(lobbyId);

			string guestName = lobby.Data.Guest;
			if (lobby.Data.Guest != null) {
				lobby.DisconnectPlayer(guestName);
			}

			return guestName;
		}

		public List<LobbyData> GetLobbyData() {
			List<LobbyData> data = new List<LobbyData>();
			foreach (Models.Lobby.LobbyModel lobby in Lobbies) {
				data.Add(lobby.Data);
			}

			return data;
		}

		public Models.Lobby.LobbyModel FindLobbyById(int lobbyId) {
			return Lobbies.SingleOrDefault(l => l.Data.LobbyId == lobbyId);
		}

		public Models.Lobby.LobbyModel FindUserLobby(string user) {
			return Lobbies.SingleOrDefault(l => l.Data.Host == user || l.Data.Guest == user);
		}
	}
}
