using System;
using System.Collections.Generic;
using System.Text;
using Czeum.Abstractions.Domain;
using Czeum.Abstractions.DTO;

namespace Czeum.Abstractions.GameServices.BoardConverter
{
    public abstract class BoardConverter<TBoard> : IBoardConverter
        where TBoard : ISerializedBoard
    {
        public abstract IMoveResult Convert(TBoard serializedBoard);

        public IMoveResult Convert(ISerializedBoard serializedBoard)
        {
            return Convert((TBoard)serializedBoard);
        }    
    }
}
