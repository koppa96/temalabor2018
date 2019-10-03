using System;
using System.Collections.Generic;
using System.Linq;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.Abstractions.GameServices;
using Czeum.Abstractions.GameServices.BoardConverter;
using Czeum.Abstractions.GameServices.BoardCreator;
using Czeum.Abstractions.GameServices.MoveHandler;
using Czeum.ChessLogic.Services;
using Czeum.DAL.Exceptions;
using Czeum.Domain.Entities;

namespace Czeum.Application.Services.ServiceContainer
{
    public class ServiceContainer : IServiceContainer
    {
        private readonly IEnumerable<IMoveHandler> moveHandlers;
        private readonly IEnumerable<IBoardCreator> boardCreators;
        private readonly IEnumerable<IBoardConverter> boardConverters;
        private readonly Random random;

        public ServiceContainer(IEnumerable<IMoveHandler> moveHandlers,
            IEnumerable<IBoardCreator> boardCreators,
            IEnumerable<IBoardConverter> boardConverters)
        {
            this.moveHandlers = moveHandlers;
            this.boardCreators = boardCreators;
            this.boardConverters = boardConverters;
            random = new Random();
        }

        public IBoardConverter FindBoardConverter(SerializedBoard serializedBoard)
        {
            return boardConverters.FirstOrDefault(x => x.GetType().BaseType?.GetGenericArguments().First() == serializedBoard.GetType())
                ?? throw new GameNotSupportedException("Could not find board converter for this game type.");
        }

        public IBoardCreator FindBoardCreator(LobbyData lobbyData)
        {
            return boardCreators.FirstOrDefault(x => x.GetType().BaseType?.GetGenericArguments().First() == lobbyData.GetType())
                ?? throw new GameNotSupportedException("Could not find board creator for this game type.");
        }

        public IMoveHandler FindMoveHandler(MoveData moveData)
        {
            return moveHandlers.FirstOrDefault(x => x.GetType().BaseType?.GetGenericArguments().First() == moveData.GetType()) 
                ?? throw new GameNotSupportedException("Could not find move handler for this game type.");
        }

        public IBoardCreator GetRandomBoardCreator()
        {
            return boardCreators.ToList()[random.Next(boardCreators.Count())];
        }
    }
}