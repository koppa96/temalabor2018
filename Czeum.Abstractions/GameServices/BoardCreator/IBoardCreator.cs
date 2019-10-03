using Czeum.Abstractions.Domain;
using Czeum.Abstractions.DTO.Lobbies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Abstractions.GameServices.BoardCreator
{
    public interface IBoardCreator
    {
        ISerializedBoard CreateBoard(LobbyData lobbyData);
        ISerializedBoard CreateDefaultBoard();
    }
}
