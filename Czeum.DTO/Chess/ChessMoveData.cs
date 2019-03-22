using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;

namespace Czeum.DTO.Chess
{
    public class ChessMoveData : MoveData
    {
        public int FromRow { get; set; }
        public int FromColumn { get; set; }
        public int ToRow { get; set; }
        public int ToColumn { get; set; }
        
        public override IGameService FindGameService(IEnumerable<IGameService> services)
        {
            return ServiceFinder.FindService(ServiceFinder.Chess, services);
        }
    }
}
