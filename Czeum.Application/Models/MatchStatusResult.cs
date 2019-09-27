using Czeum.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Application.Models
{
    public class MatchStatusResult
    {
        public MatchStatus CurrentPlayer { get; set; }
        public MatchStatus OtherPlayer { get; set; }

        public MatchStatusResult(MatchStatus currentPlayer, MatchStatus otherPlayer)
        {
            CurrentPlayer = currentPlayer;
            OtherPlayer = otherPlayer;
        }
    }
}
