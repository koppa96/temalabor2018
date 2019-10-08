using System;
using Czeum.Core.Domain;
using Czeum.Core.DTOs.Abstractions.Lobbies;

namespace Czeum.Core.GameServices.BoardCreator
{
    public abstract class BoardCreator<TLobbyData> : IBoardCreator
        where TLobbyData : LobbyData
    {
        public abstract SerializedBoard CreateDefaultBoard();
        public abstract SerializedBoard CreateBoard(TLobbyData lobbyData);

        public SerializedBoard CreateBoard(LobbyData lobbyData)
        {
            if (!lobbyData.Validate())
            {
                throw new InvalidOperationException("The lobby validation was unsuccessful.");
            }

            return CreateBoard((TLobbyData)lobbyData);
        }
    }
}
