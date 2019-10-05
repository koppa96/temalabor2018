using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Czeum.Api.Common;
using Czeum.Api.SignalR;
using Czeum.Application.Services.Lobby;
using Czeum.Application.Services.MatchService;
using Czeum.DTO;
using Czeum.DTO.Extensions;
using Czeum.DTO.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Czeum.Api.Controllers
{
    [Route(ApiResources.Matches.BasePath)]
    [ApiController]
    [Authorize]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService matchService;
        private readonly IHubContext<NotificationHub, ICzeumClient> hubContext;
        private readonly ILobbyService lobbyService;

        public MatchesController(IMatchService matchService,
            IHubContext<NotificationHub, ICzeumClient> hubContext, ILobbyService lobbyService)
        {
            this.matchService = matchService;
            this.hubContext = hubContext;
            this.lobbyService = lobbyService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchStatus>>> GetMatchesAsync()
        {
            return Ok(await matchService.GetMatchesAsync());
        }

        [HttpPost]
        public async Task<ActionResult<MatchStatus>> CreateMatchAsync([FromQuery] Guid lobbyId)
        {
            var lobby = lobbyService.GetLobby(lobbyId);
            var statuses = (await matchService.CreateMatchAsync(lobby.Content)).ToList();

            await Task.WhenAll(statuses.Skip(1)
                .Select(s => hubContext.Clients.User(s.Players
                        .Single(p => p.PlayerIndex == s.PlayerIndex).Username)
                    .MatchCreated(s)));
            
            return Ok(statuses.First());
        }

        [HttpPut("moves")]
        public async Task<ActionResult<MatchStatus>> MoveAsync([FromBody] MoveDataWrapper moveDataWrapper)
        {
            var statuses = (await matchService.HandleMoveAsync(moveDataWrapper.Content)).ToList();

            await Task.WhenAll(statuses
                .Where(s => s.PlayerIndex != s.Players.Single(p => p.Username == User.Identity.Name).PlayerIndex)
                .Select(s =>
                    hubContext.Clients
                        .User(s.Players.Single(p => p.PlayerIndex == s.PlayerIndex).Username).MatchCreated(s)));
            
            return Ok(statuses
                .Single(s => s.PlayerIndex == s.Players
                                 .Single(p => p.Username == User.Identity.Name!).PlayerIndex));
        }
    }
}