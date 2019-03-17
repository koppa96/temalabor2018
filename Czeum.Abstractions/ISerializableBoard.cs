using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Abstractions
{
    public interface ISerializableBoard<T>
    {
        T SerializeContent();
        void DeserializeContent(T serializedBoard);
    }
}
