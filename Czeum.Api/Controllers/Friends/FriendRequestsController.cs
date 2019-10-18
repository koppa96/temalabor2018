using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Api.Common;
using Czeum.Api.SignalR;
using Czeum.Core.ClientCallbacks;
using Czeum.Core.DTOs.UserManagement;
using Czeum.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Czeum.Api.Controllers.Friends
{
    [Route(ApiResources.Friends.FriendRequests.BasePath)]
    [ApiController]
    [Authorize]
    public class FriendRequestsController : ControllerBase
    {
        private readonly IFriendService friendService;
        private readonly IHubContext<NotificationHub, ICzeumClient> hubContext;

        public FriendRequestsController(IFriendService friendService,
            IHubContext<NotificationHub, ICzeumClient> hubContext)
        {
            this.friendService = friendService;
            this.hubContext = hubContext;
        }

        [HttpGet("received")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<FriendRequestDto>>> GetFriendRequestsReceivedAsync()
        {
            return Ok(await friendService.GetRequestsReceivedAsync());
        }

        [HttpGet("sent")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<FriendRequestDto>>> GetFriendRequestsSentAsync()
        {
            return Ok(await friendService.GetRequestsSentAsync());
        }

        [HttpPost("{username}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<FriendRequestDto>> SendFriendRequestAsync(string username)
        {
            var request = await friendService.AddRequestAsync(username);
            return StatusCode(201, request);
        }

        [HttpDelete("{requestId}/cancel")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> CancelFriendRequest(Guid requestId)
        {
            await friendService.RevokeRequestAsync(requestId);
            return NoContent();
        }

        [HttpDelete("{requestId}/reject")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> RejectFriendRequest(Guid requestId)
        {
            await friendService.RejectRequestAsync(requestId);
            return NoContent();
        }
    }
}