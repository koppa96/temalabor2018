using System;
using System.Collections.Generic;
using System.Linq;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.Abstractions.GameServices;
using Czeum.DAL.Entities;

namespace Czeum.Application.Services.ServiceContainer
{
    public class ServiceContainer : IServiceContainer
    {
        private readonly IEnumerable<IGameService> services;

        public ServiceContainer(IEnumerable<IGameService> services)
        {
            this.services = services;
        }

        public IGameService FindByMoveData(MoveData moveData)
        {
            var service = services.FirstOrDefault(s => Attribute.GetCustomAttributes(s.GetType())
                .Any(a => a is GameServiceAttribute attr && attr.MoveType == moveData.GetType()));

            if (service == null)
            {
                throw new GameNotSupportedException("There is no game service that can execute that move.");
            }

            return service;
        }

        public IGameService FindByLobbyData(LobbyData lobbyData)
        {
            var service = services.FirstOrDefault(s => Attribute.GetCustomAttributes(s.GetType())
                .Any(a => a is GameServiceAttribute attr && attr.LobbyType == lobbyData.GetType()));

            if (service == null)
            {
                throw new GameNotSupportedException("There is no game service that could make a board for that lobby.");
            }

            return service;
        }

        public IGameService FindBySerializedBoard(SerializedBoard serializedBoard)
        {
            var service = services.FirstOrDefault(s => Attribute.GetCustomAttributes(s.GetType())
                .Any(a => a is GameServiceAttribute attr && attr.BoardType == serializedBoard.GetType()));

            if (service == null)
            {
                throw new GameNotSupportedException("There is no game service that could process that board.");
            }

            return service;
        }

        public IGameService GetRandomService()
        {
            var serviceList = services.ToList();
            return serviceList[new Random().Next(serviceList.Count)];
        }
    }
}