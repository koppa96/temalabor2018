using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.DAL.Entities;
using Czeum.DAL.Interfaces;
using Czeum.DTO;
using Czeum.DTO.Lobby;
using Czeum.Server.Services.Lobby;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Czeum.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GameController : ControllerBase
    {
        private readonly IBoardRepository<SerializedBoard> _boardRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly ILobbyService _lobbyService;

        public GameController(IBoardRepository<SerializedBoard> boardRepository, IMatchRepository matchRepository, 
            ILobbyService lobbyService)
        {
            _boardRepository = boardRepository;
            _matchRepository = matchRepository;
            _lobbyService = lobbyService;
        }

        [HttpGet]
        [Route("/matches")]
        public ActionResult<List<MatchStatus>> GetMatches()
        {
            var matches = _matchRepository.GetMatchesOf(User.Identity.Name);
            var statusList = new List<MatchStatus>();
            foreach (var match in matches)
            {
                var status = new MatchStatus
                {
                    MatchId = match.MatchId,
                    OtherPlayer = match.GetOtherPlayerName(User.Identity.Name),
                    CurrentBoard = null,
                    State = _matchRepository.GetGameStateForMatch(match, User.Identity.Name)
                };

                statusList.Add(status);
            }

            return statusList;
        }

        [HttpGet]
        [Route("/boards/{id}")]
        public MoveResult GetBoardByMatchId(int id)
        {
            var serializedBoard = _boardRepository.GetByMatchId(id);
            return serializedBoard.ToMoveResult();
        }

        [HttpGet]
        [Route("/lobbies")]
        public ActionResult<List<LobbyData>> GetLobbies()
        {
            return _lobbyService.GetLobbyData();
        }
    }
}