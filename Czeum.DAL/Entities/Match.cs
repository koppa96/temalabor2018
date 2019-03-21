using System;
using System.ComponentModel.DataAnnotations;

namespace Czeum.DAL.Entities
{
    public class Match
    {
        [Key]
        public int MatchId { get; set; }


        public ApplicationUser Player1 { get; set; }
        public ApplicationUser Player2 { get; set; }

        public MatchState State { get; set; }
        public SerializedBoard Board { get; set; }

        public bool HasPlayer(string playerName)
        {
            return Player1.UserName == playerName || Player2.UserName == playerName;
        }

        public bool IsPlayersTurn(string playerName)
        {
            return State == MatchState.Player1Moves && Player1.UserName == playerName ||
                   State == MatchState.Player2Moves && Player2.UserName == playerName;
        }

        public int GetPlayerId(string player)
        {
            if (!HasPlayer(player))
            {
                throw new ArgumentException("The player is not playing in this match.");
            }

            return player == Player1.UserName ? 1 : 2;
        }

        public string GetOtherPlayerName(string player)
        {
            if (!HasPlayer(player))
            {
                throw new ArgumentException("The player is not playing in this match.");
            }

            return player == Player1.UserName ? Player2.UserName : Player1.UserName;
        }
    }
}
