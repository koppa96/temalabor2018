using System.Collections.Generic;
using Czeum.Abstractions.GameServices;

namespace Czeum.Abstractions.DTO {
	public abstract class LobbyData {
		public int LobbyId { get; set; }
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
