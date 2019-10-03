using Czeum.Abstractions;
using Czeum.Abstractions.Domain;
using Czeum.Abstractions.GameServices;
using Czeum.Abstractions.GameServices.BoardCreator;
using Czeum.DTO.Chess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.ChessLogic.Services
{
    public class ChessBoardCreator : BoardCreator<ChessLobbyData>
    {
        public override ISerializedBoard CreateBoard(ChessLobbyData lobbyData)
        {
            return CreateDefaultBoard();
        }

        public override ISerializedBoard CreateDefaultBoard()
        {
            return new ChessBoard(true).SerializeContent();
        }
    }
}
