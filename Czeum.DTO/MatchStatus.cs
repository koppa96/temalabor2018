using System;
using System.Collections.Generic;
using System.Text;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.DTO.Wrappers;

namespace Czeum.DTO
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
