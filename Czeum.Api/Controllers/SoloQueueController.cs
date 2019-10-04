using System.Linq;
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
            soloQueueService.JoinSoloQueue(User.Identity.Name!);

            var players = soloQueueService.PopFirstTwoPlayers();
            if (players != null)
            {
                var statuses = await gameHandler.CreateRandomMatchAsync(players);

                await Task.WhenAll(statuses.Select(s =>
                    hubContext.Clients.User(s.Players.Single(p => p.PlayerIndex == s.PlayerIndex).Username)
                        .MatchCreated(s)));
            }
            
            return Ok();
        }

        [HttpPost("leave")]
        public ActionResult LeaveQueue()
        {
            soloQueueService.LeaveSoloQueue(User.Identity.Name!);
            return Ok();
        }
    }
}