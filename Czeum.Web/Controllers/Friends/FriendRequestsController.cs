using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Core.DTOs.UserManagement;
using Czeum.Core.Services;
using Czeum.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Czeum.Web.Controllers.Friends
{
    [Route(ApiResources.Friends.FriendRequests.BasePath)]
    [ApiController]
    [Authorize]
    public class FriendRequestsController : ControllerBase
    {
        private readonly IFriendService friendService;

        public FriendRequestsController(IFriendService friendService)
        {
            this.friendService = friendService;
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

        [HttpPost("{userId}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<FriendRequestDto>> SendFriendRequestAsync(Guid userId)
        {
            var request = await friendService.AddRequestAsync(userId);
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