using Connect4.Abstractions;
using Connect4.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.DAL.Repositories
{
    public class BoardRepository<T> : IBoardRepository<T> where T : SerializedBoard
    {
        private readonly ApplicationDbContext _context;

        public BoardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void DeleteBoard(T board)
        {
            _context.Boards.Remove(board);
            _context.SaveChanges();
        }

        public T GetById(int id)
        {
            var board = _context.Boards.Find(id);
            return VaildateBoard(board);
        }

        public T GetByMatchId(int id)
        {
            var board = _context.Boards.FirstOrDefault(b => b.Match.MatchId == id);
            return VaildateBoard(board);
        }

        public void InsertBoard(T board)
        {
            _context.Boards.Add(board);
            _context.SaveChanges();
        }

        public void UpdateBoard(T board)
        {
            var outdatedBoard = GetById(board.BoardId);
            _context.Entry(outdatedBoard).CurrentValues.SetValues(board);
            _context.SaveChanges();
        }

        public void UpdateBoardData(int id, string newData)
        {
            var board = GetById(id);
            board.BoardData = newData;
            _context.SaveChanges();
        }

        private T VaildateBoard(SerializedBoard board)
        {
            if (board == null)
            {
                throw new ArgumentException("There is no board with such ID.");
            }

            if (!(board is T))
            {
                throw new ArgumentException("The board with the given ID is of a different type.");
            }

            return board as T;
        }
    }
}
