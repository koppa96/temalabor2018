using System.Threading.Tasks;
using Czeum.Api.Common;
using Czeum.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
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
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public ActionResult LeaveQueue()
        {
            soloQueueService.LeaveSoloQueue(User.Identity.Name!);
            return Ok();
        }
    }
}