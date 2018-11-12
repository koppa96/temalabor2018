using Connect4Server.Data;
using Connect4Server.Models.Lobby;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Connect4Dtos;
using LobbyModel = Connect4Server.Models.Lobby.LobbyModel;

namespace Connect4Server.Services {
	public class LobbyService {
		public List<LobbyModel> Lobbies { get; }

		public LobbyService() {
			Lobbies = new List<LobbyModel>();
		}

		/// <summary>
		/// Creates a new lobby instance and returns its id
		/// </summary>
		/// <param name="host">The player that created the lobby</param>
		/// <param name="status">Status whether the lobby is private or public</param>
		/// <returns></returns>
		public LobbyModel CreateLobby(string host, LobbyStatus status) {
			int newLobbyId = 0;
			while (Lobbies.SingleOrDefault(l => l.Data.LobbyId == newLobbyId) != null) {
				newLobbyId++;
			}

			LobbyModel model = new LobbyModel(newLobbyId, host, status);
			Lobbies.Add(model);

			return model;
		}

		public bool JoinPlayerToLobby(string player, int lobbyId) {
			LobbyModel lobby = FindLobbyById(lobbyId);
			if (lobby == null) {
				throw new ArgumentException("Invalid lobby id");
			}

			return lobby.JoinGuest(player);
		}

		public void DisconnectPlayerFromLobby(string player, int lobbyId) {
			LobbyModel lobby = FindLobbyById(lobbyId);
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
			LobbyModel lobby = FindLobbyById(lobbyId);

			string guestName = lobby.Data.Guest;
			if (lobby.Data.Guest != null) {
				lobby.DisconnectPlayer(guestName);
			}

			return guestName;
		}

		public List<LobbyData> GetLobbyData() {
			List<LobbyData> data = new List<LobbyData>();
			foreach (LobbyModel lobby in Lobbies) {
				data.Add(lobby.Data);
			}

			return data;
		}

		public LobbyModel FindLobbyById(int lobbyId) {
			foreach (LobbyModel lobby in Lobbies) {
				if (lobby.Data.LobbyId == lobbyId) {
					return lobby;
				}
			}

			return null;
		}

		public LobbyModel FindUserLobby(string user) {
			foreach (LobbyModel lobby in Lobbies) {
				if (lobby.Data.Host == user || lobby.Data.Guest == user) {
					return lobby;
				}
			}

			return null;
		}
	}
}
