using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Api.Common;
using Czeum.Core.DTOs;
using Czeum.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Czeum.Api.Controllers.Messages
{
    [Route(ApiResources.Messages.Match.BasePath)]
    [ApiController]
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
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesAsync(Guid matchId)
        {
            return Ok(await messageService.GetMessagesOfMatchAsync(matchId));
        }

        [HttpPost("{matchId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Message>> SendMessageAsync(Guid matchId, [FromBody] string message)
        {
            return Ok(await messageService.SendToMatchAsync(matchId, message));
        }
    }
}