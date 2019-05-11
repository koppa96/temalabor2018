using System.Threading.Tasks;
using Czeum.DTO;
using Czeum.Server.Services.Lobby;
using Czeum.Server.Services.SoloQueue;

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

        public static async Task<bool> LobbyJoinValidationCallbacks(this GameHub hub, ILobbyService lobbyService, ISoloQueueService soloQueueService)
        {
            if (lobbyService.FindUserLobby(hub.Context.UserIdentifier) != null)
            {
                await hub.Clients.Caller.ReceiveError(ErrorCodes.AlreadyInLobby);
                return false;
            }

            if (soloQueueService.IsQueuing(hub.Context.UserIdentifier))
            {
                await hub.Clients.Caller.ReceiveError(ErrorCodes.AlreadyQueuing);
                return false;
            }

            return true;
        } 
    }
}