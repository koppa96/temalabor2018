using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4.Abstractions
{
    [Serializable]
    public class GameNotSupportedException : Exception
    {
        public GameNotSupportedException() { }
        public GameNotSupportedException(string message) : base(message) { }
        public GameNotSupportedException(string message, Exception inner) : base(message, inner) { }
        protected GameNotSupportedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
