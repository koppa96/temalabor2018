using System.Threading.Tasks;
using Czeum.DTO;
using Czeum.Server.Services.Lobby;

namespace Czeum.Server.Hubs
{
    public static class GameHubExtensions
    {
        public static async Task<bool> LobbyValidationCallbacks(this GameHub hub, ILobbyService lobbyService, int lobbyId)
        {
            if (!lobbyService.LobbyExists(lobbyId))
            {
                await hub.Clients.Caller.ReceiveError(ErrorCodes.NoSuchLobby);
                return false;
            }
            
            if (!lobbyService.ValidateModifier(lobbyId, hub.Context.UserIdentifier))
            {
                await hub.Clients.Caller.ReceiveError(ErrorCodes.NoRightToChange);
                return false;
            }

            return true;
        }
    }
}