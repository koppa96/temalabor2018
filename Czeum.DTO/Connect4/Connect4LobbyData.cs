using System.Collections.Generic;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;

namespace Czeum.DTO.Connect4
{
    public class Connect4LobbyData : LobbyData
    {
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }
        
        public override IGameService FindGameService(IEnumerable<IGameService> services)
        {
            return ServiceFinder.FindService(ServiceFinder.Connect4, services);
        }

        public override string ValidateSettings()
        {
            if (BoardWidth < 4 || BoardWidth > 15 || BoardHeight < 4 || BoardHeight > 10)
            {
                return ErrorCodes.InvalidBoardSize;
            }

            return null;
        }
    }
}