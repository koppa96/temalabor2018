using AutoMapper;
using Czeum.Core.DTOs.Abstractions;
using Czeum.Core.DTOs.Abstractions.Lobbies;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.GameServices.ServiceMappings;

namespace Czeum.Application.Mappings
{
    public class WrapperMappings : Profile
    {
        public WrapperMappings()
        {
            CreateMap<LobbyData, LobbyDataWrapper>()
                .ForMember(dst => dst.GameIdentifier, cfg => cfg.MapFrom<LobbyGameTypeValueResolver>())
                .ForMember(dst => dst.Content, cfg => cfg.MapFrom(src => src));

            CreateMap<IMoveResult, MoveResultWrapper>()
                .ForMember(dst => dst.GameIdentifier, cfg => cfg.MapFrom<MoveResultGameTypeValueResolver>())
                .ForMember(dst => dst.Content, cfg => cfg.MapFrom(src => src));
        }
    }
}