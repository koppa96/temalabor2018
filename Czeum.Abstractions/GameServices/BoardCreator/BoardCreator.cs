using Czeum.Abstractions.Domain;
using Czeum.Abstractions.DTO.Lobbies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Abstractions.GameServices.BoardCreator
{
    public abstract class BoardCreator<TLobbyData> : IBoardCreator
        where TLobbyData : LobbyData
    {
        public abstract ISerializedBoard CreateDefaultBoard();
        public abstract ISerializedBoard CreateBoard(TLobbyData lobbyData);

        public ISerializedBoard CreateBoard(LobbyData lobbyData)
        {
            if (!lobbyData.Validate())
            {
                throw new InvalidOperationException("The lobby validation was unsuccessful.");
            }

            return CreateBoard((TLobbyData)lobbyData);
        }
    }
}
