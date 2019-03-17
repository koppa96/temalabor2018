using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4.Abstractions
{
    public class GameServiceAttribute : Attribute
    {
        public string Identifier { get; }

        public GameServiceAttribute(string identifier)
        {
            Identifier = identifier;
        }
    }
}
