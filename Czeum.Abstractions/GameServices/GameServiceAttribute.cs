using System;

namespace Czeum.Abstractions.GameServices
{
    /// <summary>
    /// A custom attribute for GameServices to determine which kinds of DTO-s they can operate with.
    /// </summary>
    public class GameServiceAttribute : Attribute
    {
        /// <summary>
        /// The type of the MoveData the GameService can execute.
        /// </summary>
        public Type MoveType { get; }

        /// <summary>
        /// The type of the LobbyData the GameService can use to create matches.
        /// </summary>
        public Type LobbyType { get; }

        /// <summary>
        /// The type of the serialized board the GameService can convert to MoveResult.
        /// </summary>
        public Type BoardType { get; }

        public GameServiceAttribute(Type moveType, Type lobbyType, Type boardType)
        {
            MoveType = moveType;
            LobbyType = lobbyType;
            BoardType = boardType;
        }
    }
}
