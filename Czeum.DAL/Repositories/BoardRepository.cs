using Czeum.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.DAL.Entities;
using Czeum.DAL.Interfaces;

namespace Czeum.DAL.Repositories
{
    public class BoardRepository<T> : IBoardRepository
    {
        private readonly ApplicationDbContext _context;

        public BoardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void DeleteBoard(SerializedBoard board)
        {
            _context.Boards.Remove(board);
        }

        public SerializedBoard GetById(int id)
        {
            return _context.Boards.Find(id);
        }

        public SerializedBoard GetByMatchId(int id)
        {
            return _context.Boards.FirstOrDefault(b => b.Match.MatchId == id);
        }

        public void InsertBoard(SerializedBoard board)
        {
            _context.Boards.Add(board);
        }

        public void UpdateBoard(SerializedBoard board)
        {
            var outdatedBoard = GetById(board.BoardId);
            _context.Entry(outdatedBoard).CurrentValues.SetValues(board);
        }

        public void UpdateBoardData(int id, string newData)
        {
            var board = GetById(id);
            board.BoardData = newData;
        }

        public MoveResult GetMoveResultByMatchId(int matchId)
        {
            var board = GetByMatchId(matchId);
            return board.ToMoveResult();
        }
    }
}
