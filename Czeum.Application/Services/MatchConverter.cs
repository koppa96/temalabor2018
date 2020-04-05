using System.Linq;
using AutoMapper;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.Enums;
using Czeum.Core.Services;
using Czeum.Domain.Entities;
using Czeum.Domain.Enums;

namespace Czeum.Application.Services
{
    public class MatchConverter : IMatchConverter
    {
        private readonly IServiceContainer serviceContainer;
        private readonly IMapper mapper;

        public MatchConverter(IServiceContainer serviceContainer, IMapper mapper)
        {
            this.serviceContainer = serviceContainer;
            this.mapper = mapper;
        }
        
        public MatchStatus ConvertFor(Match match, string user)
        {
            var converter = serviceContainer.FindBoardConverter(match.Board);
            
            return new MatchStatus
            {
                Id = match.Id,
                CurrentPlayerIndex = match.CurrentPlayerIndex,
                PlayerIndex = match.Users.Single(u => u.User.UserName == user).PlayerIndex,
                CurrentBoard = mapper.Map<MoveResultWrapper>(converter.Convert(match.Board)),
                Players = match.Users.Select(um => new Player { Username = um.User.UserName, PlayerIndex = um.PlayerIndex }),
                State = ConvertState(match, user),
                Winner = match.Winner?.UserName,
                CreateDate = match.CreateTime,
                LastMoveDate = match.LastMove
            };
        }

        private GameState ConvertState(Match match, string user)
        {
            if (match.State == MatchState.InProgress)
            {
                return match.CurrentPlayerIndex == match.Users.First(um => um.User.UserName == user).PlayerIndex 
                    ? GameState.YourTurn : GameState.EnemyTurn;
            }

            if (match.Winner == null)
            {
                return GameState.Draw;
            }

            return match.Winner.UserName == user ? GameState.Won : GameState.Lost;
        }
    }
}