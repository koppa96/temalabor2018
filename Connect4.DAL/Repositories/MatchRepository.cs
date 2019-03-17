using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Connect4.Abstractions;

namespace Connect4.DAL.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly ApplicationDbContext _context;

        public MatchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int GetPlayerIdByName(string name, int matchId)
        {
            if (_context.Matches.Find(matchId).Player1.UserName == name)
            {
                return 1;
            }

            if (_context.Matches.Find(matchId).Player2.UserName == name)
            {
                return 2;
            }

            throw new ArgumentException("The given player is not playing in");
        }
    }
}
