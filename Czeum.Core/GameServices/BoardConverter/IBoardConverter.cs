using Czeum.Core.Domain;
using Czeum.Core.DTOs.Abstractions;

namespace Czeum.Core.GameServices.BoardConverter
{
    public interface IBoardConverter
    {
        IMoveResult Convert(SerializedBoard serializedBoard);
    }
}
