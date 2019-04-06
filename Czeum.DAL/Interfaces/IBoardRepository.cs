using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Czeum.DAL.Entities;

namespace Czeum.DAL.Interfaces
{
    public interface IBoardRepository
    {
        SerializedBoard GetByMatchId(int id);
        void InsertBoard(SerializedBoard board);
        void UpdateBoard(SerializedBoard board);
        void DeleteBoard(SerializedBoard board);
        void UpdateBoardData(int id, string newData);
        SerializedBoard GetById(int id);
    }
}
