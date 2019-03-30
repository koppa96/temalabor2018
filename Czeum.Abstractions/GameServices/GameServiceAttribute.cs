using System;

namespace Czeum.Abstractions.GameServices
{
    public class GameServiceAttribute : Attribute
    {
        public Type MoveType { get; }
        public Type LobbyType { get; }

        public GameServiceAttribute(Type moveType, Type lobbyType)
        {
            MoveType = moveType;
            LobbyType = lobbyType;
        }
    }
}
