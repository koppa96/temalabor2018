using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Api.Common;
using Czeum.Api.SignalR;
using Czeum.Application.Services.FriendService;
using Czeum.DTO.UserManagement;
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
        public async Task<ActionResult<IEnumerable<FriendRequestDto>>> GetFriendRequestsReceivedAsync()
        {
            return Ok(await friendService.GetRequestsReceivedAsync());
        }

        [HttpGet("sent")]
        public async Task<ActionResult<IEnumerable<FriendRequestDto>>> GetFriendRequestsSentAsync()
        {
            return Ok(await friendService.GetRequestsSentAsync());
        }

        [HttpPost("{username}")]
        public async Task<ActionResult<FriendRequestDto>> SendFriendRequestAsync(string username)
        {
            return Ok(await friendService.AddRequestAsync(username));
        }

        [HttpDelete("{requestId}/cancel")]
        public async Task<ActionResult> CancelFriendRequest(Guid requestId)
        {
            await friendService.RemoveRequestAsync(requestId);

            // TODO: Notify other user
            return NoContent();
        }

        [HttpDelete("{requestId}/reject")]
        public async Task<ActionResult> RejectFriendRequest(Guid requestId)
        {
            await friendService.RemoveRequestAsync(requestId);

            // TODO: Notify other user
            return NoContent();
        }
    }
}