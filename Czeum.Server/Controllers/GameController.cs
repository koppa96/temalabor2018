using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.DAL.Entities;
using Czeum.DAL.Interfaces;
using Czeum.DTO;
using Czeum.DTO.UserManagement;
using Czeum.Server.Hubs;
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
        private readonly IBoardRepository<SerializedBoard> _boardRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly ILobbyService _lobbyService;
        private readonly IFriendRepository _friendRepository;
        private readonly IOnlineUserTracker _onlineUserTracker;

        public GameController(IBoardRepository<SerializedBoard> boardRepository, IMatchRepository matchRepository, 
            ILobbyService lobbyService, IFriendRepository friendRepository, IOnlineUserTracker onlineUserTracker)
        {
            _boardRepository = boardRepository;
            _matchRepository = matchRepository;
            _lobbyService = lobbyService;
            _friendRepository = friendRepository;
            _onlineUserTracker = onlineUserTracker;
        }

        [HttpGet]
        [Route("/matches")]
        public ActionResult<List<MatchStatus>> GetMatches()
        {
            return _matchRepository.GetMatchesOf(User.Identity.Name).Select(m => new MatchStatus
            {
                MatchId = m.MatchId,
                OtherPlayer = m.GetOtherPlayerName(User.Identity.Name),
                CurrentBoard = null,
                State = m.GetGameStateForPlayer(User.Identity.Name)
            }).ToList();
        }

        [HttpGet]
        [Route("/boards/{id}")]
        public ActionResult<MoveResult> GetBoardByMatchId(int id)
        {
            var serializedBoard = _boardRepository.GetByMatchId(id);
            return serializedBoard.ToMoveResult();
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
            return _friendRepository.GetRequestsReceivedBy(User.Identity.Name);
        }

        [HttpGet]
        [Route("/myrequests")]
        public ActionResult<List<string>> GetSentRequests()
        {
            return _friendRepository.GetRequestsSentBy(User.Identity.Name);
        }

        [HttpGet]
        [Route("/friends")]
        public ActionResult<List<Friend>> GetFriends()
        {
            return _friendRepository.GetFriendsOf(User.Identity.Name)
                .Select(f => new Friend { IsOnline = _onlineUserTracker.IsOnline(f), Username = f })
                .ToList();
        }
    }
}