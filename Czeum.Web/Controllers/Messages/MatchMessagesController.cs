using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Paging;
using Czeum.Core.Services;
using Czeum.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Czeum.Web.Controllers.Messages
{
    [Route(ApiResources.Messages.Match.BasePath)]
    [ApiController]
    [Authorize]
    public class MatchMessagesController : ControllerBase
    {
        private readonly IMessageService messageService;

        public MatchMessagesController(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        [HttpGet("{matchId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<RollListDto<Message>>> GetMessagesAsync(Guid matchId, [FromQuery]Guid? oldestId, [FromQuery]int count = 25)
        {
            return Ok(await messageService.GetMessagesOfMatchAsync(matchId, oldestId, count));
        }

        [HttpPost("{matchId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Message>> SendMessageAsync(Guid matchId, [FromBody]string message)
        {
            return Ok(await messageService.SendToMatchAsync(matchId, message));
        }
    }
}