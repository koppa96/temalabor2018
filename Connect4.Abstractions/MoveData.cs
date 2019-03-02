using System;
using System.Collections.Generic;
using System.Linq;

namespace Connect4.Abstractions
{
    public abstract class MoveData
    {
        public int MatchId { get; set; }

        public abstract IGameService FindGameService(IEnumerable<IGameService> services);

        protected IGameService FindGameServiceTyped<T>(IEnumerable<IGameService> services)
        {
            var service = services.FirstOrDefault(s => s is T);

            if (service == null)
            {
                throw new GameNotSupportedException("The server is not running the requested game service.");
            }

            return service;
        }
    }
}
