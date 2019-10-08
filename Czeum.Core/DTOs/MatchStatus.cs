using System;
using System.Collections.Generic;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.Enums;

namespace Czeum.Core.DTOs
{
    /// <summary>
    /// Represents a match stored in the server.
    /// </summary>
    public class MatchStatus
    {
        public Guid Id { get; set; }
        public int CurrentPlayerIndex { get; set; }
        public int PlayerIndex { get; set; }
        public IEnumerable<Player> Players { get; set; }
        public MoveResultWrapper CurrentBoard { get; set; }
        public GameState State { get; set; }
        public string Winner { get; set; }
    }
}
