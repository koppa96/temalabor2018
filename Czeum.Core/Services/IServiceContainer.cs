using Czeum.Core.Domain;
using Czeum.Core.DTOs.Abstractions;
using Czeum.Core.DTOs.Abstractions.Lobbies;
using Czeum.Core.GameServices.BoardConverter;
using Czeum.Core.GameServices.BoardCreator;
using Czeum.Core.GameServices.MoveHandler;
using System;
using System.Collections.Generic;

namespace Czeum.Core.Services
{
    /// <summary>
    /// Interface for IGameService containers
    /// </summary>
    public interface IServiceContainer
    {
        IMoveHandler FindMoveHandler(MoveData moveData);
        
        IBoardCreator FindBoardCreator(LobbyData lobbyData);

        IBoardConverter FindBoardConverter(SerializedBoard serializedBoard);
        
        IBoardCreator GetRandomBoardCreator();
        IEnumerable<Type> GetRegisteredBoardTypes();
        IEnumerable<Type> GetRegisteredMoveHandlerTypes();
    }
}