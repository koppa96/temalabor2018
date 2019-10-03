using System;
using AutoMapper;
using Czeum.Abstractions.DTO;
using Czeum.Domain.Entities;
using Czeum.Domain.Enums;
using Czeum.DTO;
using Czeum.DTO.Wrappers;

namespace Czeum.Application.Extensions
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

        public static GameState GetGameStateForPlayer(this Match match, string player)
        {
            if (!match.HasPlayer(player))
            {
                throw new ArgumentException("The player is not playing this match");
            }

            var playerId = match.GetPlayerId(player);
            switch (match.State)
            {
                case MatchState.Draw:
                    return GameState.Draw;
                case MatchState.Player1Moves:
                    return playerId == 1 ? GameState.YourTurn : GameState.EnemyTurn;
                case MatchState.Player2Moves:
                    return playerId == 1 ? GameState.EnemyTurn : GameState.YourTurn;
                case MatchState.Player1Won:
                    return playerId == 1 ? GameState.YouWon : GameState.EnemyWon;
                case MatchState.Player2Won:
                    return playerId == 1 ? GameState.EnemyWon : GameState.YouWon;
                default:
                    throw new NotSupportedException("There is an unhandled MatchState that can't be converted to GameState.");
            }
        }
    }
}