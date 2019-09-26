using System;
using System.Collections.Generic;

namespace Czeum.Abstractions.DTO.Lobbies {
    /// <summary>
    /// An abstract base class for lobbies.
    /// </summary>
	public abstract class LobbyData {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Host { get; set; }
		public string Guest { get; set; }
		public LobbyAccess Access { get; set; }
		public List<string> InvitedPlayers { get; set; }
		public bool Empty => Host == null;

		protected LobbyData()
		{
			InvitedPlayers = new List<string>();
			Access = LobbyAccess.FriendsOnly;
		}
		
		public abstract string ValidateSettings();
	}
}
