using System;
using Czeum.ChessLogic;
using Czeum.DAL.Entities;
using Czeum.DAL.Interfaces;

namespace Czeum.Tests.ChessLogic
{
    public class DummyRepository : IBoardRepository<SerializedChessBoard>
    {
        private readonly SerializedChessBoard board;

        public DummyRepository()
        {
            board = new ChessBoard(true).SerializeContent();
            board.BoardId = 1;
            board.MatchId = 1;
        }
        
        public SerializedChessBoard GetByMatchId(int id)
        {
            if (board.MatchId == id)
            {
                return board;
            }
            
            throw new ArgumentException("No such board.");
        }

        public int InsertBoard(SerializedChessBoard board)
        {
            throw new NotImplementedException();
        }

        public void UpdateBoard(SerializedChessBoard board)
        {
            throw new NotImplementedException();
        }

        public void DeleteBoard(SerializedChessBoard board)
        {
            throw new NotImplementedException();
        }

        public void UpdateBoardData(int id, string newData)
        {
            if (board.BoardId == id)
            {
                board.BoardData = newData;
            }
        }

        public SerializedChessBoard GetById(int id)
        {
            return GetByMatchId(id);
        }
    }
}