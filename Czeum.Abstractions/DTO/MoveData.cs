using System;
using System.Collections.Generic;
using System.Linq;
using Czeum.Abstractions.GameServices;

namespace Czeum.Abstractions.DTO
{
    public abstract class MoveData
    {
        public int MatchId { get; set; }

        public abstract IGameService FindGameService(IEnumerable<IGameService> services);

        protected IGameService FindGameServiceByIdentifier(IEnumerable<IGameService> services, string id)
        {
            var service = services.FirstOrDefault(s => Attribute.GetCustomAttributes(s.GetType())
                .Any(a => a is GameServiceAttribute attr && attr.Identifier == id));

            if (service == null)
            {
                throw new GameNotSupportedException("The server does not have the required service at the moment.");
            }

            return service;
        }
    }
}
