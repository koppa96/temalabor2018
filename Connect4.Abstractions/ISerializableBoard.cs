using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4.Abstractions
{
    public interface ISerializableBoard<T>
    {
        T ToSerialized();
        void FillFromSerialized(T serializedBoard);
    }
}
