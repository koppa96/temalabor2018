using System.Threading.Tasks;
using Czeum.DTO;

namespace Czeum.Server.Hubs
{
    public partial class GameHub
    {
        public async Task JoinSoloQueue()
        {
            if (_lobbyService.FindUserLobby(Context.UserIdentifier) != null)
            {
                await Clients.Caller.ReceiveError(ErrorCodes.AlreadyInLobby);
                return;
            }
            
            _soloQueueService.JoinSoloQueue(Context.UserIdentifier);
            var players = _soloQueueService.PopFirstTwoPlayers();
            if (players != null)
            {
                var statuses = await _gameHandler.CreateRandomMatchAsync(players[0], players[1]);

                await Clients.User(players[0]).MatchCreated(statuses[players[0]]);
                await Clients.User(players[1]).MatchCreated(statuses[players[1]]);
            }
        }

        public void LeaveSoloQueue()
        {
            _soloQueueService.LeaveSoloQueue(Context.UserIdentifier);
        }
    }
}