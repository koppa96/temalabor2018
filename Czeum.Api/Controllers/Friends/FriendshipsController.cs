using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Api.Common;
using Czeum.Api.SignalR;
using Czeum.Application.Services.FriendService;
using Czeum.Application.Services.OnlineUsers;
using Czeum.DTO.UserManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Czeum.Api.Controllers.Friends
{
    [Route(ApiResources.Friends.Friendships.BasePath)]
    [ApiController]
    public class FriendshipsController : ControllerBase
    {
        private readonly IFriendService friendService;
        private readonly IOnlineUserTracker onlineUserTracker;
        private readonly IHubContext<NotificationHub, ICzeumClient> hubContext;

        public FriendshipsController(IFriendService friendService,
            IOnlineUserTracker onlineUserTracker,
            IHubContext<NotificationHub, ICzeumClient> hubContext)
        {
            this.friendService = friendService;
            this.onlineUserTracker = onlineUserTracker;
            this.hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FriendDto>>> GetFriendsAsync()
        {
            return Ok((await friendService.GetFriendsOfUserAsync(User.Identity.Name))
                .Select(f => new FriendDto
                {
                    IsOnline = onlineUserTracker.IsOnline(f),
                    Username = f
                }));
        }

        [HttpDelete("{friendshipId}")]
        public async Task<ActionResult> RemoveFriendAsync(Guid friendshipId)
        {
            await friendService.RemoveFriendAsync(friendshipId);
            return NoContent();
        }

        [HttpPost("{requestId}")]
        public async Task<ActionResult<FriendDto>> AcceptRequestAsync(Guid requestId)
        {
            var result = await friendService.AcceptRequestAsync(requestId);

            await hubContext.Clients.User(result.Receiver.Username).FriendAdded(result.Sender);
            return StatusCode(201, result.Receiver);
        }
    }
}