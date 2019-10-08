using AutoMapper;
using Czeum.Core.DTOs.Abstractions;
using Czeum.Core.DTOs.Abstractions.Lobbies;
using Czeum.Core.DTOs.Extensions;
using Czeum.Core.DTOs.Wrappers;

namespace Czeum.Application.Mappings
{
    public class WrapperMappings : Profile
    {
        public WrapperMappings()
        {
            CreateMap<LobbyData, LobbyDataWrapper>()
                .ForMember(dst => dst.GameType, cfg => cfg.MapFrom(src => src.GetGameType()))
                .ForMember(dst => dst.Content, cfg => cfg.MapFrom(src => src));

            CreateMap<IMoveResult, MoveResultWrapper>()
                .ForMember(dst => dst.GameType, cfg => cfg.MapFrom(src => src.GetGameType()))
                .ForMember(dst => dst.Content, cfg => cfg.MapFrom(src => src));
        }
    }
}