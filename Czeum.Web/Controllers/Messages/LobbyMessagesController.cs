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
    [Route(ApiResources.Messages.Lobby.BasePath)]
    [ApiController]
    [Authorize]
    public class LobbyMessagesController : ControllerBase
    {
        private readonly IMessageService messageService;
        public LobbyMessagesController(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        [HttpGet("{lobbyId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<RollListDto<Message>>> GetMessages(Guid lobbyId, [FromQuery]Guid? oldestId, [FromQuery]int count = 25)
        {
            return Ok(await messageService.GetMessagesOfLobbyAsync(lobbyId, oldestId, count));
        }

        [HttpPost("{lobbyId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Message>> SendMessage(Guid lobbyId, [FromBody] string message)
        {
            return Ok(await messageService.SendToLobbyAsync(lobbyId, message));
        }
    }
}