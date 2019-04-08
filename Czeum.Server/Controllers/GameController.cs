using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.DAL;
using Czeum.DAL.Entities;
using Czeum.DAL.Interfaces;
using Czeum.DTO;
using Czeum.DTO.UserManagement;
using Czeum.Server.Hubs;
using Czeum.Server.Services.FriendService;
using Czeum.Server.Services.GameHandler;
using Czeum.Server.Services.Lobby;
using Czeum.Server.Services.OnlineUsers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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
        public GameController(ILobbyService lobbyService, IOnlineUserTracker onlineUserTracker, IGameHandler gameHandler,
            IFriendService friendService)
        {
            _lobbyService = lobbyService;
            _onlineUserTracker = onlineUserTracker;
            _gameHandler = gameHandler;
            _friendService = friendService;
        }

        [HttpGet]
        [Route("/matches")]
        public ActionResult<List<MatchStatus>> GetMatches()
        {
            return _gameHandler.GetMatchesOf(User.Identity.Name);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("/boards/{id}")]
        public ActionResult<MoveResult> GetBoardByMatchId(int id)
        {
            var result = _gameHandler.GetBoardByMatchId(id);

            if (result == null)
            {
                return NotFound();
            }
            
            return result;
        }

        [HttpGet]
        [Route("/lobbies")]
        public ActionResult<List<LobbyData>> GetLobbies()
        {
            return _lobbyService.GetLobbies();
        }

        [HttpGet]
        [Route("/requests")]
        public ActionResult<List<string>> GetFriendRequests()
        {
            return _friendService.GetRequestsReceivedBy(User.Identity.Name);
        }

        [HttpGet]
        [Route("/sent-requests")]
        public ActionResult<List<string>> GetSentRequests()
        {
            return _friendService.GetRequestsSentBy(User.Identity.Name);
        }

        [HttpGet]
        [Route("/friends")]
        public ActionResult<List<Friend>> GetFriends()
        {
            return _friendService.GetFriendsOf(User.Identity.Name)
                .Select(f => new Friend { IsOnline = _onlineUserTracker.IsOnline(f), Username = f })
                .ToList();
        }
    }
}