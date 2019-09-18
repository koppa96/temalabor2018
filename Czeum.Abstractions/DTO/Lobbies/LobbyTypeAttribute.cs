using System;

namespace Czeum.Abstractions.DTO.Lobbies
{
    public class LobbyTypeAttribute : Attribute
    {
        public Type LobbyType { get; }

        public LobbyTypeAttribute(Type lobbyType)
        {
            LobbyType = lobbyType;
        }
    }
}