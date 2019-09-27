using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.Abstractions.GameServices;
using Czeum.Abstractions.GameServices.BoardConverter;
using Czeum.Abstractions.GameServices.BoardCreator;
using Czeum.Abstractions.GameServices.MoveHandler;
using Czeum.Domain.Entities;

namespace Czeum.Application.Services.ServiceContainer
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
    }
}