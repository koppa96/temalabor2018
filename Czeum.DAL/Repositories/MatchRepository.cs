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

        public Dictionary<string, MatchStatus> CreateMatchStatuses(int matchId, MoveResult moveResult)
        {
            var statuses = new Dictionary<string, MatchStatus>();
            var match = _context.Matches.Find(matchId);

            statuses[match.Player1.UserName] = match.ToMatchStatus(match.Player1.UserName);
            statuses[match.Player2.UserName] = match.ToMatchStatus(match.Player2.UserName);
            statuses[match.Player1.UserName].CurrentBoard = moveResult;
            statuses[match.Player2.UserName].CurrentBoard = moveResult;
            
            return statuses;
        }

        public List<MatchStatus> GetMatchesOf(string player)
        {
            return _context.Matches.Include("Player1").Include("Player2")
                .Where(m => m.HasPlayer(player))
                .Select(m => m.ToMatchStatus(player))
                .ToList();
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

        public Dictionary<string, MatchStatus> CreateMatch(LobbyData lobbyData, int boardId)
        {
            return CreateMatch(lobbyData.Host, lobbyData.Guest, boardId);
        }

        public Dictionary<string, MatchStatus> CreateMatch(string player1, string player2, int boardId)
        {
            var match = new Match
            {
                Player1 = _context.Users.SingleOrDefault(u => u.UserName == player1),
                Player2 = _context.Users.SingleOrDefault(u => u.UserName == player2),
                Board = _context.Boards.Find(boardId),
                State = MatchState.Player1Moves
            };

            if (match.Player1 == null)
            {
                throw new ArgumentOutOfRangeException(nameof(player1), "There is no such player.");
            }

            if (match.Player2 == null)
            {
                throw new ArgumentOutOfRangeException(nameof(player2), "There is no such player");
            }

            if (match.Board == null)
            {
                throw new ArgumentOutOfRangeException(nameof(boardId), "The board id must be a valid board id");
            }

            _context.Matches.Add(match);
            _context.SaveChanges();
            
            return new Dictionary<string, MatchStatus>
            {
                { player1, match.ToMatchStatus(player1) },
                { player2, match.ToMatchStatus(player2) }
            };
        }
    }
}
