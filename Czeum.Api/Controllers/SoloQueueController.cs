using System.Linq;
using System.Threading.Tasks;
using Czeum.Api.Common;
using Czeum.Api.SignalR;
using Czeum.Application.Services.MatchService;
using Czeum.Application.Services.SoloQueue;
using Czeum.ClientCallback;
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
        private readonly IMatchService matchService;
        private readonly IHubContext<NotificationHub, ICzeumClient> hubContext;

        public SoloQueueController(ISoloQueueService soloQueueService,
            IMatchService matchService,
            IHubContext<NotificationHub, ICzeumClient> hubContext)
        {
            this.soloQueueService = soloQueueService;
            this.matchService = matchService;
            this.hubContext = hubContext;
        }
        
        [HttpPost("join")]
        public async Task<ActionResult> JoinQueue()
        {
            soloQueueService.JoinSoloQueue(User.Identity.Name!);

            var players = soloQueueService.PopFirstTwoPlayers();
            if (players != null)
            {
                var statuses = await matchService.CreateRandomMatchAsync(players);

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