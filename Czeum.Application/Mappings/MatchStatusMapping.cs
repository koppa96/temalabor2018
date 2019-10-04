using AutoMapper;
using Czeum.Abstractions.DTO;
using Czeum.Domain.Entities;
using Czeum.Domain.Enums;
using Czeum.DTO;

namespace Czeum.Application.Mappings
{
    public class MatchStatusMapping : Profile
    {
        public MatchStatusMapping()
        {
            CreateMap<MatchState, GameState>()
                .ConstructUsing(value => value switch
                {
                    MatchState.InProgress => GameState.InProgress,
                    MatchState.Finished => GameState.Finished
                });
            
            CreateMap<UserMatch, MatchStatus>()
                .ForMember()
        }
    }
}