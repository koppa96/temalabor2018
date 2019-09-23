using Czeum.Api.Common;
using Czeum.Application.Services.SoloQueue;
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

        public SoloQueueController(ISoloQueueService soloQueueService)
        {
            this.soloQueueService = soloQueueService;
        }
        
        [HttpPost("join")]
        public ActionResult JoinQueue()
        {
            soloQueueService.JoinSoloQueue(User.Identity.Name);
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