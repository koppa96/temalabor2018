using System;
using System.Collections.Generic;
using System.Linq;

namespace Czeum.Abstractions
{
    public abstract class MoveData
    {
        public int MatchId { get; set; }

        public abstract IGameService FindGameService(IEnumerable<IGameService> services);

        protected IGameService FindGameServiceByIdentifier(IEnumerable<IGameService> services, string id)
        {
            return services.FirstOrDefault(s => Attribute.GetCustomAttributes(s.GetType())
                .Any(a => a is GameServiceAttribute attr && attr.Identifier == id));
        }
    }
}
