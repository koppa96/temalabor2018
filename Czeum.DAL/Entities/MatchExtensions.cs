using System;

namespace Czeum.DAL.Entities
{
    public static class MatchExtensions
    {
        public static bool HasPlayer(this Match match, string playerName)
        {
            return match.Player1.UserName == playerName || match.Player2.UserName == playerName;
        }

        public static bool IsPlayersTurn(this Match match, string playerName)
        {
            return match.State == MatchState.Player1Moves && match.Player1.UserName == playerName ||
                   match.State == MatchState.Player2Moves && match.Player2.UserName == playerName;
        }

        public static int GetPlayerId(this Match match, string player)
        {
            if (!match.HasPlayer(player))
            {
                throw new ArgumentException("The player is not playing in this match.");
            }

            return player == match.Player1.UserName ? 1 : 2;
        }

        public static string GetOtherPlayerName(this Match match, string player)
        {
            if (!match.HasPlayer(player))
            {
                throw new ArgumentException("The player is not playing in this match.");
            }

            return player == match.Player1.UserName ? match.Player2.UserName : match.Player1.UserName;
        }
    }
}