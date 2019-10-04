using Czeum.Abstractions.Domain;
using System;
using System.Collections.Generic;

namespace Czeum.Abstractions.DTO.Lobbies {
    /// <summary>
    /// An abstract base class for lobbies.
    /// </summary>
	public abstract class LobbyData : IAuditedEntity {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Host { get; set; }
		public List<string> Guests { get; set; }
		public LobbyAccess Access { get; set; }
		public List<string> InvitedPlayers { get; set; }
		public bool Empty => Host == null;
		public abstract int MinimumPlayerCount { get; }
		public abstract int MaximumPlayerCount { get; }

        public DateTime Created { get; set; }

        public DateTime LastModified { get; set; }

        protected LobbyData()
		{
			InvitedPlayers = new List<string>();
			Access = LobbyAccess.FriendsOnly;
            Created = DateTime.UtcNow;
            LastModified = Created;
		}
        
        public bool Validate()
        {
	        var playerCount = Guests.Count + 1;
	        return playerCount >= MinimumPlayerCount && playerCount <= MaximumPlayerCount && ValidateSettings();
        }

        protected abstract bool ValidateSettings();
    }
}
