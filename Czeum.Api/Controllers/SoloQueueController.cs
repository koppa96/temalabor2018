using System.Threading.Tasks;
using Czeum.Api.Common;
using Czeum.Api.SignalR;
using Czeum.Application.Services.GameHandler;
using Czeum.Application.Services.SoloQueue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Czeum.Api.Controllers
{
    [Route(ApiResources.SoloQueue.BasePath)]
    [ApiController]
    [Authorize]
    public class SoloQueueController : ControllerBase
    {
        private readonly ISoloQueueService soloQueueService;
        private readonly IGameHandler gameHandler;
        private readonly IHubContext<NotificationHub, ICzeumClient> hubContext;

        public SoloQueueController(ISoloQueueService soloQueueService,
            IGameHandler gameHandler,
            IHubContext<NotificationHub, ICzeumClient> hubContext)
        {
            this.soloQueueService = soloQueueService;
            this.gameHandler = gameHandler;
            this.hubContext = hubContext;
        }
        
        [HttpPost("join")]
        public async Task<ActionResult> JoinQueue()
        {
            soloQueueService.JoinSoloQueue(User.Identity.Name);

            var players = soloQueueService.PopFirstTwoPlayers();
            if (players != null)
            {
                var statuses = await gameHandler.CreateRandomMatchAsync(players[0], players[1]);

                await hubContext.Clients.User(players[0]).MatchCreated(statuses[players[0]]);
                await hubContext.Clients.User(players[1]).MatchCreated(statuses[players[1]]);
            }
            
            return Ok();
        }

        [HttpPost("leave")]
        public ActionResult LeaveQueue()
        {
            soloQueueService.LeaveSoloQueue(User.Identity.Name);
            return Ok();
        }
    }
}