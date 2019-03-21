using System;

namespace Czeum.Abstractions.GameServices
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
