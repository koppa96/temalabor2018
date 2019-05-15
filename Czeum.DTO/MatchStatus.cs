using System;
using System.Collections.Generic;
using System.Text;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;

namespace Czeum.DTO
{
    /// <summary>
    /// Represents a match stored in the server.
    /// </summary>
    public class MatchStatus
    {
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public string OtherPlayer { get; set; }
        public MoveResult CurrentBoard { get; set; }
        public GameState State { get; set; }
    }
}
