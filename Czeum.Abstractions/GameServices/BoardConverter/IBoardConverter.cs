using Czeum.Abstractions.Domain;
using Czeum.Abstractions.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Abstractions.GameServices.BoardConverter
{
    public interface IBoardConverter
    {
        IMoveResult Convert(ISerializedBoard serializedBoard);
    }
}
