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
        private readonly IHubContext<NotificationHub, ICzeumClient> hubContext;
        private readonly IMatchService matchService;

        public MatchMessagesController(IMessageService messageService,
            IHubContext<NotificationHub, ICzeumClient> hubContext,
            IMatchService matchService)
        {
            this.messageService = messageService;
            this.hubContext = hubContext;
            this.matchService = matchService;
        }

        [HttpGet("{matchId}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesAsync(Guid matchId)
        {
            return Ok(await messageService.GetMessagesOfMatchAsync(matchId));
        }

        [HttpPost("{matchId}")]
        public async Task<ActionResult<Message>> SendMessageAsync(Guid matchId, [FromBody] string message)
        {
            var sentMessage = await messageService.SendToMatchAsync(matchId, message);
            var others = await matchService.GetOthersInMatchAsync(matchId);

            await hubContext.Clients.Users(others.ToList()).ReceiveMatchMessage(matchId, sentMessage);
            return Ok(message);
        }
    }
}