using System;

namespace Czeum.Abstractions.DTO.Lobbies
{
    public class GameTypeAttribute : Attribute
    {
        public Type LobbyType { get; }
        public Type MoveDataType { get; }
        public Type MoveResultType { get; }

        public GameTypeAttribute(Type lobbyType, Type moveDataType, Type moveResultType)
        {
            LobbyType = lobbyType;
            MoveDataType = moveDataType;
            MoveResultType = moveResultType;
        }
    }
}