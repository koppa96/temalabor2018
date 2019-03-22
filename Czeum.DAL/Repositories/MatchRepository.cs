using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.DAL.Entities;
using Czeum.DAL.Interfaces;
using Czeum.DTO;
using Microsoft.EntityFrameworkCore;

namespace Czeum.DAL.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly ApplicationDbContext _context;

        public MatchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public MatchStatus CreateMatchStatusFor(int matchId, string player, MoveResult moveResult)
        {
            var match = _context.Matches.Find(matchId);

            if (match == null)
            {
                throw new ArgumentException("There is no match with the given id.");
            }

            int playerId = match.GetPlayerId(player);

            MatchStatus status = new MatchStatus()
            {
                CurrentBoard = moveResult,
                MatchId = matchId,
                OtherPlayer = playerId == 1 ? match.Player1.UserName : match.Player2.UserName
            };

            switch (match.State)
            {
                case MatchState.Player1Moves:
                    status.State = playerId == 1 ? GameState.YourTurn : GameState.EnemyTurn;
                    break;
                case MatchState.Player2Moves:
                    status.State = playerId == 2 ? GameState.YourTurn : GameState.EnemyTurn;
                    break;
                case MatchState.Draw:
                    status.State = GameState.Draw;
                    break;
                case MatchState.Player1Won:
                    status.State = playerId == 1 ? GameState.YouWon : GameState.EnemyWon;
                    break;
                case MatchState.Player2Won:
                    status.State = playerId == 2 ? GameState.YouWon : GameState.EnemyWon;
                    break;
            }

            return status;
        }

        public List<Match> GetMatchesOf(string player)
        {
            return _context.Matches.Include("Player1").Include("Player2")
                .Where(m => m.Player1.UserName == player || m.Player2.UserName == player)
                .ToList();
        }

        public GameState GetGameStateForMatch(Match match, string player)
        {
            switch (match.State)
            {
                case MatchState.Draw:
                    return GameState.Draw;
                case MatchState.Player1Moves:
                    return match.Player1.UserName == player
                        ? GameState.YourTurn
                        : GameState.EnemyTurn;
                case MatchState.Player2Moves:
                    return match.Player2.UserName == player
                        ? GameState.YourTurn
                        : GameState.EnemyTurn;
                case MatchState.Player1Won:
                    return match.Player1.UserName == player
                        ? GameState.YouWon
                        : GameState.EnemyWon;
                case MatchState.Player2Won:
                    return match.Player2.UserName == player
                        ? GameState.YouWon
                        : GameState.EnemyWon;
                default:
                    throw new ArgumentException("There is no GameState for this MatchState.");
            }
        }

        public string GetOtherPlayer(int matchId, string player)
        {
            var match = _context.Matches.Find(matchId);

            if (match.HasPlayer(player))
            {
                return player == match.Player1.UserName ? match.Player2.UserName : match.Player1.UserName;
            }
            
            throw new ArgumentException("The player is not playing in the match.");
        }

        public Match GetMatchById(int matchId)
        {
            return _context.Matches.Include("Player1").Include("Player2")
                .FirstOrDefault(m => m.MatchId == matchId);
        }

        public void UpdateMatchByStatus(int matchId, Status status)
        {
            if (status == Status.Requested || status == Status.Fail)
            {
                return;
            }

            var match = _context.Matches.Find(matchId);

            switch (status)
            {
                case Status.Success:
                    match.State = match.State == MatchState.Player1Moves
                        ? MatchState.Player2Moves
                        : MatchState.Player1Moves;
                    break;
                case Status.Draw:
                    match.State = MatchState.Draw;
                    break;
                case Status.Win:
                    match.State = match.State == MatchState.Player1Moves
                        ? MatchState.Player1Won
                        : MatchState.Player2Won;
                    break;
            }

            _context.SaveChanges();
        }

        public int CreateMatch(LobbyData lobbyData, int boardId)
        {
            var match = new Match
            {
                Player1 = _context.Users.SingleOrDefault(u => u.UserName == lobbyData.Host),
                Player2 = _context.Users.SingleOrDefault(u => u.UserName == lobbyData.Guest),
                Board = _context.Boards.Find(boardId),
                State = MatchState.Player1Moves
            };

            _context.Matches.Add(match);
            return match.MatchId;
        }
    }
}
