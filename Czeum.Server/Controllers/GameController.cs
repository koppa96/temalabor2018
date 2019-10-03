using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.DTO;
using Czeum.DTO.UserManagement;
using Czeum.Server.Services.FriendService;
using Czeum.Server.Services.GameHandler;
using Czeum.Server.Services.Lobby;
using Czeum.Server.Services.MessageService;
using Czeum.Server.Services.OnlineUsers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Czeum.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GameController : ControllerBase
    {
        private readonly ILobbyService _lobbyService;
        private readonly IOnlineUserTracker _onlineUserTracker;
        private readonly IGameHandler _gameHandler;
        private readonly IFriendService _friendService;
        private readonly IMessageService _messageService;

        public GameController(ILobbyService lobbyService, IOnlineUserTracker onlineUserTracker, IGameHandler gameHandler,
            IFriendService friendService, IMessageService messageService)
        {
            _lobbyService = lobbyService;
            _onlineUserTracker = onlineUserTracker;
            _gameHandler = gameHandler;
            _friendService = friendService;
            _messageService = messageService;
        }

        [HttpGet]
        [Route("matches")]
        public async Task<ActionResult<List<MatchStatus>>> GetMatchesAsync()
        {
            return await _gameHandler.GetMatchesOfPlayerAsync(User.Identity.Name);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("boards/{id}")]
        public async Task<ActionResult<MoveResult>> GetBoardByMatchIdAsync(int id)
        {
            var result = await _gameHandler.GetBoardByMatchIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }
            
            return result;
        }

        [HttpGet]
        [Route("lobbies")]
        public ActionResult<List<LobbyData>> GetLobbies()
        {
            return _lobbyService.GetLobbies();
        }

        [HttpGet]
        [Route("requests")]
        public async Task<ActionResult<List<string>>> GetFriendRequestsAsync()
        {
            return await _friendService.GetRequestsReceivedByUserAsync(User.Identity.Name);
        }

        [HttpGet]
        [Route("sent-requests")]
        public async Task<ActionResult<List<string>>> GetSentRequests()
        {
            return await _friendService.GetRequestsSentByUserAsync(User.Identity.Name);
        }

        [HttpGet]
        [Route("friends")]
        public async Task<ActionResult<List<FriendDto>>> GetFriends()
        {
            return (await _friendService.GetFriendsOfUserAsync(User.Identity.Name))
                .Select(f => new FriendDto { IsOnline = _onlineUserTracker.IsOnline(f), Username = f })
                .ToList();
        }

        [HttpGet]
        [Route("messages/{id}")]
        public async Task<ActionResult<List<Message>>> GetMessages(int id)
        {
            return await _messageService.GetMessagesOfMatchAsync(id);
        }
    }
}