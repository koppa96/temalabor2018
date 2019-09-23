using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.Api.Common;
using Czeum.Application.Services.Lobby;
using Czeum.DTO.Extensions;
using Czeum.DTO.Lobbies;
using Czeum.DTO.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Czeum.Api.Controllers
{
    [Route(ApiResources.Lobbies.BasePath)]
    [ApiController]
    public class LobbiesController : ControllerBase
    {
        private readonly ILobbyService lobbyService;

        public LobbiesController(ILobbyService lobbyService)
        {
            this.lobbyService = lobbyService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<LobbyDataWrapper>> GetLobbies()
        {
            return lobbyService.GetLobbies();
        }

        [HttpPost]
        public ActionResult<LobbyDataWrapper> CreateLobby([FromBody] CreateLobbyDto dto)
        {
            return lobbyService.CreateAndAddLobby(
                dto.GameType, 
                User.Identity.Name, 
                dto.LobbyAccess, 
                dto.Name);
        }

        [HttpPut("{lobbyId}")]
        public ActionResult<LobbyDataWrapper> UpdateLobby(int lobbyId, [FromBody] LobbyDataWrapper wrapper)
        {
            lobbyService.UpdateLobbySettings(wrapper);
            return lobbyService.GetLobby(lobbyId);
        }

        [HttpPost("{lobbyId}/leave")]
        public ActionResult LeaveLobby(int lobbyId)
        {
            lobbyService.DisconnectPlayerFromLobby(User.Identity.Name, lobbyId);
            return Ok();
        }

        [HttpPost("{lobbyId}/invite")]
        public ActionResult<LobbyDataWrapper> InvitePlayer(int lobbyId, [FromQuery] string playerName)
        {
            lobbyService.InvitePlayerToLobby(lobbyId, playerName);
            return lobbyService.GetLobby(lobbyId);
        }

        [HttpDelete("{lobbyId}/invite")]
        public ActionResult<LobbyDataWrapper> CancelInvitation(int lobbyId, [FromQuery] string playerName)
        {
            lobbyService.CancelInviteFromLobby(lobbyId, playerName);
            return lobbyService.GetLobby(lobbyId);
        }

        [HttpPost("{lobbyId}/join")]
        public async Task<ActionResult<LobbyDataWrapper>> JoinLobby(int lobbyId)
        {
            await lobbyService.JoinPlayerToLobbyAsync(User.Identity.Name, lobbyId);
            return lobbyService.GetLobby(lobbyId);
        }

        [HttpPost("{lobbyId}/kick")]
        public ActionResult<LobbyDataWrapper> KickGuest(int lobbyId)
        {
            lobbyService.KickGuest(lobbyId);
            return Ok();
        }
    }
}