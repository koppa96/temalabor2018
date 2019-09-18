using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.Api.Common;
using Czeum.Application.Services.Lobby;
using Czeum.DTO.Lobbies;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IEnumerable<LobbyData>> GetLobbies()
        {
            return lobbyService.GetLobbies();
        }

        [HttpPost]
        public ActionResult<LobbyData> CreateLobby([FromBody] CreateLobbyDto dto)
        {
            return lobbyService.CreateAndAddLobby(
                dto.LobbyType, 
                User.Identity.Name, 
                dto.LobbyAccess, 
                dto.Name);
        }

        [HttpPut("{lobbyId}")]
        public ActionResult<LobbyData> UpdateLobby(int lobbyId, [FromBody] LobbyData lobbyData)
        {
            lobbyService.UpdateLobbySettings(lobbyData);
            return lobbyService.GetLobby(lobbyId);
        }

        [HttpPost("{lobbyId}/leave")]
        public ActionResult LeaveLobby(int lobbyId)
        {
            lobbyService.DisconnectPlayerFromLobby(User.Identity.Name, lobbyId);
            return Ok();
        }

        [HttpPost("{lobbyId}/invite")]
        public ActionResult<LobbyData> InvitePlayer(int lobbyId, [FromQuery] string playerName)
        {
            lobbyService.InvitePlayerToLobby(lobbyId, playerName);
            return lobbyService.GetLobby(lobbyId);
        }

        [HttpDelete("{lobbyId}/invite")]
        public ActionResult<LobbyData> CancelInvitation(int lobbyId, [FromQuery] string playerName)
        {
            lobbyService.CancelInviteFromLobby(lobbyId, playerName);
            return lobbyService.GetLobby(lobbyId);
        }

        [HttpPost("{lobbyId}/join")]
        public async Task<ActionResult<LobbyData>> JoinLobby(int lobbyId)
        {
            await lobbyService.JoinPlayerToLobbyAsync(User.Identity.Name, lobbyId);
            return lobbyService.GetLobby(lobbyId);
        }

        [HttpPost("{lobbyId}/kick")]
        public ActionResult<LobbyData> KickGuest(int lobbyId)
        {
            lobbyService.KickGuest(lobbyId);
            return Ok();
        }
    }
}