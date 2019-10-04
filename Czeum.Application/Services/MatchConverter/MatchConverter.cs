using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices.BoardConverter;
using Czeum.Application.Services.ServiceContainer;
using Czeum.Domain.Entities;
using Czeum.Domain.Enums;
using Czeum.Domain.Services;
using Czeum.DTO;
using Czeum.DTO.Wrappers;

namespace Czeum.Application.Services.MatchConverter
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
                CurrentBoard = mapper.Map<MoveResultWrapper>(converter.Convert(match.Board)),
                Players = match.Users.Select(um => new Player { Username = um.User.UserName, PlayerIndex = um.PlayerIndex }),
                State = ConvertState(match, user),
                Winner = match.Winner?.UserName
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