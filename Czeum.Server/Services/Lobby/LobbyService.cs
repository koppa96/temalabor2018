using Czeum.Server.Models.Lobby;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Czeum.DTO.Lobby;

namespace Czeum.Server.Services.Lobby {
	public class LobbyService : ILobbyService
    {
        private readonly List<LobbyModel> lobbies;

		public LobbyService() {
			lobbies = new List<LobbyModel>();
		}

		/// <summary>
		/// Creates a new lobby instance and returns its id
		/// </summary>
		/// <param name="host">The player that created the lobby</param>
		/// <param name="access">Status whether the lobby is private or public</param>
		/// <returns></returns>
		public LobbyModel CreateLobby(string host, LobbyAccess access) {
			int newLobbyId = 0;
			while (lobbies.SingleOrDefault(l => l.Data.LobbyId == newLobbyId) != null) {
				newLobbyId++;
			}

			LobbyModel model = new LobbyModel(newLobbyId, host, access);
			lobbies.Add(model);

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
				lobbies.Remove(lobby);
			}			
		}

		public void DeleteLobby(int lobbyId) {
			lobbies.Remove(FindLobbyById(lobbyId));
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
			return lobbies.Select(l => l.Data).ToList();
        }

		public LobbyModel FindLobbyById(int lobbyId) {
			return lobbies.SingleOrDefault(l => l.Data.LobbyId == lobbyId);
		}

		public LobbyModel FindUserLobby(string user) {
			return lobbies.SingleOrDefault(l => l.Data.Host == user || l.Data.Guest == user);
		}
	}
}
