using Czeum.Core.Domain;
using Czeum.Core.DTOs.Abstractions;
using System;

namespace Czeum.Core.GameServices.BoardConverter
{
    public abstract class BoardConverter<TBoard, TMoveResult> : IBoardConverter
        where TBoard : SerializedBoard
        where TMoveResult : IMoveResult
    {
        public abstract TMoveResult Convert(TBoard serializedBoard);

        public IMoveResult Convert(SerializedBoard serializedBoard)
        {
            if (!(serializedBoard is TBoard))
            {
                throw new NotSupportedException("This converter can not convert this board.");
            }

            return Convert((TBoard)serializedBoard);
        }    
    }
}
