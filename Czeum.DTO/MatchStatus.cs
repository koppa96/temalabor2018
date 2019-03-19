using System;
using System.Collections.Generic;
using System.Text;
using Czeum.Abstractions;

namespace Czeum.DTO
{
    public class MatchStatus
    {
        public int MatchId { get; set; }
        public string OtherPlayer { get; set; }
        public MoveResult CurrentBoard { get; set; }
        public GameState State { get; set; }
    }
}
