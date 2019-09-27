using Czeum.Abstractions;
using Czeum.Abstractions.GameServices;
using Czeum.Abstractions.GameServices.BoardCreator;
using Czeum.Domain.Entities;
using Czeum.DTO.Connect4;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Connect4Logic.Services
{
    public class Connect4BoardCreator : BoardCreator<Connect4LobbyData>
    {
        public override ISerializedBoard CreateBoard(Connect4LobbyData lobbyData)
        {
            return new Connect4Board(lobbyData.BoardWidth, lobbyData.BoardHeight).SerializeContent();
        }

        public override ISerializedBoard CreateDefaultBoard()
        {
            var lobby = new Connect4LobbyData();
            return CreateBoard(lobby);
        }
    }
}
