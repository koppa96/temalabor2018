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

        public SoloQueueController(ISoloQueueService soloQueueService,
            IMatchService matchService)
        {
            this.soloQueueService = soloQueueService;
            this.matchService = matchService;
        }
        
        [HttpPost("join")]
        public async Task<ActionResult> JoinQueue()
        {
            soloQueueService.JoinSoloQueue(User.Identity.Name!);

            // TODO: Solo queuing only handles 2 player matches!
            var players = soloQueueService.PopFirstTwoPlayers();
            if (players != null)
            {
                await matchService.CreateRandomMatchAsync(players);
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