using System;
using Czeum.Connect4Logic;
using Czeum.DAL.Entities;
using Czeum.DAL.Interfaces;

namespace Czeum.Tests.Connect4Logic
{
    public class DummyRepository : IBoardRepository<SerializedConnect4Board>
    {
        private readonly SerializedConnect4Board board;
        
        public DummyRepository()
        {
            board = new Connect4Board().SerializeContent();
            board.MatchId = 1;
            board.BoardId = 1;
        }
        
        public SerializedConnect4Board GetByMatchId(int id)
        {
            if (id == 1)
            {
                return board;
            }
            
            throw new ArgumentException("No such board.");
        }

        public int InsertBoard(SerializedConnect4Board board)
        {
            throw new NotImplementedException();
        }

        public void UpdateBoard(SerializedConnect4Board board)
        {
            throw new NotImplementedException();
        }

        public void DeleteBoard(SerializedConnect4Board board)
        {
            throw new NotImplementedException();
        }

        public void UpdateBoardData(int id, string newData)
        {
            if (id == board.BoardId)
            {
                board.BoardData = newData;
            }
        }

        public SerializedConnect4Board GetById(int id)
        {
            return GetByMatchId(id);
        }
    }
}