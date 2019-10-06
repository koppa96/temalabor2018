using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.Api.Common;
using Czeum.Api.SignalR;
using Czeum.Application.Services.Lobby;
using Czeum.ClientCallback;
using Czeum.DTO.Extensions;
using Czeum.DTO.Lobbies;
using Czeum.DTO.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Czeum.Api.Controllers
{
    [Route(ApiResources.Lobbies.BasePath)]
    [ApiController]
    [Authorize]
    public class LobbiesController : ControllerBase
    {
        private readonly ILobbyService lobbyService;
        private readonly IHubContext<NotificationHub, ICzeumClient> hubContext;

        public LobbiesController(ILobbyService lobbyService, IHubContext<NotificationHub, ICzeumClient> hubContext)
        {
            this.lobbyService = lobbyService;
            this.hubContext = hubContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<LobbyDataWrapper>> GetLobbies()
        {
            return lobbyService.GetLobbies();
        }

        [HttpPost]
        public async Task<ActionResult<LobbyDataWrapper>> CreateLobbyAsync([FromBody] CreateLobbyDto dto)
        {
            return StatusCode(201, await lobbyService.CreateAndAddLobbyAsync(
                dto.GameType,  
                dto.LobbyAccess, 
                dto.Name));
        }

        [HttpPut("{lobbyId}")]
        public async Task<ActionResult<LobbyDataWrapper>> UpdateLobbyAsync([FromBody] LobbyDataWrapper wrapper)
        {
            return Ok(await lobbyService.UpdateLobbySettingsAsync(wrapper));
        }

        [HttpPost("current/leave")]
        public async Task<ActionResult> LeaveLobbyAsync()
        {
            await lobbyService.DisconnectFromCurrentLobbyAsync();
            return Ok();
        }

        [HttpPost("{lobbyId}/invite")]
        public async Task<ActionResult<LobbyDataWrapper>> InvitePlayerAsync(Guid lobbyId, [FromQuery] string playerName)
        {
            lobbyService.InvitePlayerToLobby(lobbyId, playerName);
            var lobby = lobbyService.GetLobby(lobbyId);

            await hubContext.Clients.User(playerName).ReceiveLobbyInvite(lobby);
            await hubContext.Clients.All.LobbyChanged(lobby);
            return lobbyService.GetLobby(lobbyId);
        }

        [HttpDelete("{lobbyId}/invite")]
        public async Task<ActionResult<LobbyDataWrapper>> CancelInvitationAsync(Guid lobbyId, [FromQuery] string playerName)
        {
            lobbyService.CancelInviteFromLobby(lobbyId, playerName);
            var lobby = lobbyService.GetLobby(lobbyId);

            await hubContext.Clients.All.LobbyChanged(lobby);
            return lobbyService.GetLobby(lobbyId);
        }

        [HttpPost("{lobbyId}/join")]
        public async Task<ActionResult<LobbyDataWrapper>> JoinLobbyAsync(Guid lobbyId)
        {
            await lobbyService.JoinToLobbyAsync(lobbyId);
            var lobby = lobbyService.GetLobby(lobbyId);

            await hubContext.Clients.All.LobbyChanged(lobby);
            return lobby;
        }

        [HttpPost("{lobbyId}/kick/{guestName}")]
        public async Task<ActionResult<LobbyDataWrapper>> KickGuestAsync(Guid lobbyId, string guestName)
        {
            return Ok(await lobbyService.KickGuestAsync(lobbyId, guestName));
        }
    }
}