using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.Api.Common;
using Czeum.Api.SignalR;
using Czeum.Application.Services.Lobby;
using Czeum.DTO.Extensions;
using Czeum.DTO.Lobbies;
using Czeum.DTO.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;

namespace Czeum.Api.Controllers
{
    [Route(ApiResources.Lobbies.BasePath)]
    [ApiController]
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
            var lobby = lobbyService.CreateAndAddLobby(
                dto.GameType, 
                User.Identity.Name, 
                dto.LobbyAccess, 
                dto.Name);

            await hubContext.Clients.AllExcept(User.Identity.Name).LobbyAdded(lobby);
            return StatusCode(201, lobby);
        }

        [HttpPut("{lobbyId}")]
        public async Task<ActionResult<LobbyDataWrapper>> UpdateLobbyAsync(int lobbyId, [FromBody] LobbyDataWrapper wrapper)
        {
            lobbyService.UpdateLobbySettings(wrapper, User.Identity.Name);
            var lobby = lobbyService.GetLobby(lobbyId);

            await hubContext.Clients.AllExcept(User.Identity.Name).LobbyChanged(lobby);
            return Ok(lobby);
        }

        [HttpPost("{lobbyId}/leave")]
        public async Task<ActionResult> LeaveLobbyAsync(int lobbyId)
        {
            lobbyService.DisconnectPlayerFromLobby(User.Identity.Name, lobbyId);
            var lobby = lobbyService.GetLobby(lobbyId);

            if (lobby == null)
            {
                await hubContext.Clients.All.LobbyDeleted(lobbyId);
            }
            else
            {
                await hubContext.Clients.All.LobbyChanged(lobby);
            }
            
            return Ok();
        }

        [HttpPost("{lobbyId}/invite")]
        public async Task<ActionResult<LobbyDataWrapper>> InvitePlayerAsync(int lobbyId, [FromQuery] string playerName)
        {
            lobbyService.InvitePlayerToLobby(lobbyId, User.Identity.Name, playerName);
            var lobby = lobbyService.GetLobby(lobbyId);

            await hubContext.Clients.User(playerName).ReceiveLobbyInvite(lobby);
            await hubContext.Clients.All.LobbyChanged(lobby);
            return lobbyService.GetLobby(lobbyId);
        }

        [HttpDelete("{lobbyId}/invite")]
        public async Task<ActionResult<LobbyDataWrapper>> CancelInvitationAsync(int lobbyId, [FromQuery] string playerName)
        {
            lobbyService.CancelInviteFromLobby(lobbyId, playerName);
            var lobby = lobbyService.GetLobby(lobbyId);

            await hubContext.Clients.All.LobbyChanged(lobby);
            return lobbyService.GetLobby(lobbyId);
        }

        [HttpPost("{lobbyId}/join")]
        public async Task<ActionResult<LobbyDataWrapper>> JoinLobbyAsync(int lobbyId)
        {
            await lobbyService.JoinPlayerToLobbyAsync(User.Identity.Name, lobbyId);
            var lobby = lobbyService.GetLobby(lobbyId);

            await hubContext.Clients.All.LobbyChanged(lobby);
            return lobby;
        }

        [HttpPost("{lobbyId}/kick")]
        public async Task<ActionResult<LobbyDataWrapper>> KickGuestAsync(int lobbyId)
        {
            lobbyService.KickGuest(lobbyId, User.Identity.Name);
            var lobby = lobbyService.GetLobby(lobbyId);

            await hubContext.Clients.AllExcept(User.Identity.Name).LobbyChanged(lobby);
            return Ok(lobby);
        }
    }
}