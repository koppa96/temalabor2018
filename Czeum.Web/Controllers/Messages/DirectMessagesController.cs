using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Paging;
using Czeum.Core.Services;
using Czeum.Web.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Czeum.Web.Controllers.Messages
{
    [Route(ApiResources.Messages.BasePath)]
    [ApiController]
    public class DirectMessagesController : ControllerBase
    {
        private readonly IMessageService messageService;

        public DirectMessagesController(IMessageService messageService)
        {
            this.messageService = messageService;
        }
        
        [HttpGet("{friendshipId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<RollListDto<Message>>> GetMessagesAsync(Guid friendshipId, [FromQuery]Guid? oldestId, [FromQuery]int count = 25)
        {
            return Ok(await messageService.GetMessagesOfFrienshipAsync(friendshipId, oldestId, count));
        }

        [HttpPost("{friendshipId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Message>> SendMessageAsync(Guid friendshipId, [FromBody]string message)
        {
            return Ok(await messageService.SendToFriendAsync(friendshipId, message));
        }
    }
}