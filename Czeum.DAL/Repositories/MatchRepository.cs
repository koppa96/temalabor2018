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
                .SingleOrDefault(m => m.MatchId == matchId);
        }

        public Match UpdateMatchByStatus(int matchId, Status status)
        {
            var match = GetMatchById(matchId);
            
            if (status == Status.Requested || status == Status.Fail)
            {
                return match;
            }

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

            return match;
        }

        public Match CreateMatch(LobbyData lobbyData, SerializedBoard board)
        {
            return CreateMatch(lobbyData.Host, lobbyData.Guest, board);
        }

        public Match CreateMatch(string player1, string player2, SerializedBoard board)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board), "The board can not be null.");
            }
            
            var match = new Match
            {
                Player1 = _context.Users.SingleOrDefault(u => u.UserName == player1),
                Player2 = _context.Users.SingleOrDefault(u => u.UserName == player2),
                Board = board,
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

            _context.Matches.Add(match);
            return match;
        }
    }
}
