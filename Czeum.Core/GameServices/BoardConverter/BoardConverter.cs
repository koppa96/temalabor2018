using Czeum.Core.Domain;
using Czeum.Core.DTOs.Abstractions;

namespace Czeum.Core.GameServices.BoardConverter
{
    public abstract class BoardConverter<TBoard> : IBoardConverter
        where TBoard : SerializedBoard
    {
        public abstract IMoveResult Convert(TBoard serializedBoard);

        public IMoveResult Convert(SerializedBoard serializedBoard)
        {
            return Convert((TBoard)serializedBoard);
        }    
    }
}
