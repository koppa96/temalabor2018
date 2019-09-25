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
    [Route(ApiResources.Friends.FriendRequests.BasePath)]
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

        [HttpDelete("{friendName}")]
        public async Task<ActionResult> RemoveFriendAsync(string friendName)
        {
            await friendService.RemoveFriendAsync(User.Identity.Name, friendName);
            return NoContent();
        }

        [HttpPost("{friendName}")]
        public async Task<ActionResult<FriendDto>> AcceptRequestAsync(string friendName)
        {
            await friendService.AcceptRequestAsync(friendName, User.Identity.Name);

            await hubContext.Clients.User(friendName).FriendAdded(new FriendDto
            {
                IsOnline = true,
                Username = User.Identity.Name
            });
            
            return StatusCode(201, new FriendDto
            {
                IsOnline = onlineUserTracker.IsOnline(friendName),
                Username = friendName
            });
        }
    }
}