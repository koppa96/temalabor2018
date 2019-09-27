using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Czeum.Api.Common;
using Czeum.Api.SignalR;
using Czeum.Application.Services.GameHandler;
using Czeum.DTO;
using Czeum.DTO.Extensions;
using Czeum.DTO.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Czeum.Api.Controllers
{
    [Route(ApiResources.Matches.BasePath)]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IGameHandler gameHandler;
        private readonly IHubContext<NotificationHub, ICzeumClient> hubContext;

        public MatchesController(IGameHandler gameHandler,
            IHubContext<NotificationHub, ICzeumClient> hubContext)
        {
            this.gameHandler = gameHandler;
            this.hubContext = hubContext;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchStatus>>> GetMatchesAsync()
        {
            return Ok(await gameHandler.GetMatchesOfPlayerAsync(User.Identity.Name));
        }

        [HttpPost]
        public async Task<ActionResult<MatchStatus>> CreateMatchAsync([FromBody] LobbyDataWrapper lobbyDataWrapper)
        {
            var statuses = await gameHandler.CreateMatchAsync(lobbyDataWrapper.Content);

            await hubContext.Clients.User(lobbyDataWrapper.Content.Guest)
                .MatchCreated(statuses.OtherPlayer);
            return Ok(statuses.CurrentPlayer);
        }

        [HttpPut("moves")]
        public async Task<ActionResult<MatchStatus>> MoveAsync([FromBody] MoveDataWrapper moveDataWrapper)
        {
            var statuses = await gameHandler.HandleMoveAsync(moveDataWrapper.Content);

            await hubContext.Clients.User(statuses.CurrentPlayer.OtherPlayer).ReceiveResult(statuses.OtherPlayer);
            return statuses.CurrentPlayer;
        }
    }
}