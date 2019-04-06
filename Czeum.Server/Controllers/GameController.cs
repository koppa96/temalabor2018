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
        private readonly IUnitOfWork _unitOfWork;

        public GameController(ILobbyService lobbyService, IOnlineUserTracker onlineUserTracker, IUnitOfWork unitOfWork)
        {
            _lobbyService = lobbyService;
            _onlineUserTracker = onlineUserTracker;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("/matches")]
        public ActionResult<List<MatchStatus>> GetMatches()
        {
            return _unitOfWork.MatchRepository.GetMatchesOf(User.Identity.Name);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("/boards/{id}")]
        public ActionResult<MoveResult> GetBoardByMatchId(int id)
        {
            var serializedBoard = _unitOfWork.BoardRepository.GetByMatchId(id);

            if (serializedBoard == null)
            {
                return NotFound();
            }
            
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
            return _unitOfWork.FriendRepository.GetRequestsReceivedBy(User.Identity.Name);
        }

        [HttpGet]
        [Route("/sent-requests")]
        public ActionResult<List<string>> GetSentRequests()
        {
            return _unitOfWork.FriendRepository.GetRequestsSentBy(User.Identity.Name);
        }

        [HttpGet]
        [Route("/friends")]
        public ActionResult<List<Friend>> GetFriends()
        {
            return _unitOfWork.FriendRepository.GetFriendsOf(User.Identity.Name)
                .Select(f => new Friend { IsOnline = _onlineUserTracker.IsOnline(f), Username = f })
                .ToList();
        }
    }
}