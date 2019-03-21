using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Czeum.DAL.Entities;

namespace Czeum.DAL.Interfaces
{
    public interface IBoardRepository<T> where T : SerializedBoard
    {
        T GetByMatchId(int id);
        void InsertBoard(T board);
        void UpdateBoard(T board);
        void DeleteBoard(T board);
        void UpdateBoardData(int id, string newData);
        T GetById(int id);
    }
}
