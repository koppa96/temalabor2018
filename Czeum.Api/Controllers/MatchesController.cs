using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Api.Common;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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