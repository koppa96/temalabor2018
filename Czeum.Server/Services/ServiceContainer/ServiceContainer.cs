using System;
using System.Collections.Generic;
using System.Linq;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;

namespace Czeum.Server.Services.ServiceContainer
{
    public class ServiceContainer : IServiceContainer
    {
        private readonly IEnumerable<IGameService> _services;

        public ServiceContainer(IEnumerable<IGameService> services)
        {
            _services = services;
        }

        public IGameService FindService(MoveData moveData)
        {
            var service = _services.FirstOrDefault(s => Attribute.GetCustomAttributes(s.GetType())
                .Any(a => a is GameServiceAttribute attr && attr.MoveType == moveData.GetType()));

            if (service == null)
            {
                throw new GameNotSupportedException("There is no game service that can execute that move.");
            }

            return service;
        }

        public IGameService FindService(LobbyData lobbyData)
        {
            var service = _services.FirstOrDefault(s => Attribute.GetCustomAttributes(s.GetType())
                .Any(a => a is GameServiceAttribute attr && attr.LobbyType == lobbyData.GetType()));

            if (service == null)
            {
                throw new GameNotSupportedException("There is no game service that could make a board for that lobby.");
            }

            return service;
        }

        public IGameService GetRandomService()
        {
            var serviceList = _services.ToList();
            return serviceList[new Random().Next(serviceList.Count)];
        }
    }
}