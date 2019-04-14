using System;

namespace Czeum.Abstractions.GameServices
{
    public class GameServiceAttribute : Attribute
    {
        public Type MoveType { get; }
        public Type LobbyType { get; }
        public Type BoardType { get; }

        public GameServiceAttribute(Type moveType, Type lobbyType, Type boardType)
        {
            MoveType = moveType;
            LobbyType = lobbyType;
            BoardType = boardType;
        }
    }
}
