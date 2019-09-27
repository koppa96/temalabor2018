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
            if (lobbyData.Host == null || lobbyData.Guest == null)
            {
                throw new InvalidOperationException("A match can only be created from a full lobby.");
            }

            return CreateBoard((TLobbyData)lobbyData);
        }
    }
}
