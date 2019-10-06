using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Api.Common;
using Czeum.Api.SignalR;
using Czeum.Application.Services.MatchService;
using Czeum.Application.Services.MessageService;
using Czeum.ClientCallback;
using Czeum.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesAsync(Guid matchId)
        {
            return Ok(await messageService.GetMessagesOfMatchAsync(matchId));
        }

        [HttpPost("{matchId}")]
        public async Task<ActionResult<Message>> SendMessageAsync(Guid matchId, [FromBody] string message)
        {
            return Ok(await messageService.SendToMatchAsync(matchId, message));
        }
    }
}