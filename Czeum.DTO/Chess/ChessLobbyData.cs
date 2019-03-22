using System.Collections.Generic;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;

namespace Czeum.DTO.Chess
{
    public class ChessLobbyData : LobbyData
    {
        public override IGameService FindGameService(IEnumerable<IGameService> services)
        {
            return ServiceFinder.FindService(ServiceFinder.Chess, services);
        }

        public override string ValidateSettings()
        {
            return null;
        }
    }
}