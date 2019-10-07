using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Czeum.Api.Common;
using Czeum.Api.SignalR;
using Czeum.Application.Services.Lobby;
using Czeum.Application.Services.MatchService;
using Czeum.ClientCallback;
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

        public MatchesController(IMatchService matchService)
        {
            this.matchService = matchService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchStatus>>> GetMatchesAsync()
        {
            return Ok(await matchService.GetMatchesAsync());
        }

        [HttpPost]
        public async Task<ActionResult<MatchStatus>> CreateMatchAsync([FromQuery] Guid lobbyId)
        {
            return Ok(await matchService.CreateMatchAsync(lobbyId));
        }

        [HttpPut("moves")]
        public async Task<ActionResult<MatchStatus>> MoveAsync([FromBody] MoveDataWrapper moveDataWrapper)
        {
            return Ok(await matchService.HandleMoveAsync(moveDataWrapper.Content));
        }
    }
}