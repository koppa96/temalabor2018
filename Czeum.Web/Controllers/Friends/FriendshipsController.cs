using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Core.ClientCallbacks;
using Czeum.Core.DTOs.UserManagement;
using Czeum.Core.Services;
using Czeum.Web.Common;
using Czeum.Web.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Czeum.Web.Controllers.Friends
{
    [Route(ApiResources.Friends.Friendships.BasePath)]
    [ApiController]
    [Authorize]
    public class FriendshipsController : ControllerBase
    {
        private readonly IFriendService friendService;
        private readonly IHubContext<NotificationHub, ICzeumClient> hubContext;

        public FriendshipsController(IFriendService friendService,
            IHubContext<NotificationHub, ICzeumClient> hubContext)
        {
            this.friendService = friendService;
            this.hubContext = hubContext;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<FriendDto>>> GetFriendsAsync()
        {
            return Ok(await friendService.GetFriendsOfUserAsync(User.Identity.Name));
        }

        [HttpDelete("{friendshipId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> RemoveFriendAsync(Guid friendshipId)
        {
            await friendService.RemoveFriendAsync(friendshipId);
            return NoContent();
        }

        [HttpPost("{requestId}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<FriendDto>> AcceptRequestAsync(Guid requestId)
        {
            var result = await friendService.AcceptRequestAsync(requestId);
            return StatusCode(201, result);
        }
    }
}