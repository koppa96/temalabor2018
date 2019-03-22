using System;
using System.Collections.Generic;
using System.Linq;

namespace Czeum.Abstractions.GameServices
{
    public static class ServiceFinder
    {
        public const string Connect4 = "Connect4";
        public const string Chess = "Chess";

        public static IGameService FindService(string identifier, IEnumerable<IGameService> services)
        {
            var service = services.FirstOrDefault(s => Attribute.GetCustomAttributes(s.GetType())
                .Any(a => a is GameServiceAttribute attr && attr.Identifier == identifier));

            if (service == null)
            {
                throw new GameNotSupportedException("The server does not have the required service at the moment.");
            }

            return service;
        }
    }
}
