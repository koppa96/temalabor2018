using System;
using System.Collections.Generic;
using Czeum.Core.Domain;
using Czeum.Core.Enums;

namespace Czeum.Core.DTOs.Abstractions.Lobbies {
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
			Guests = new List<string>();
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

        public abstract bool ValidateSettings();
    }
    
    public abstract class LobbyDataWithSettings<TSettings> : LobbyData
    {
	    public TSettings Settings { get; set; }
    }
}
