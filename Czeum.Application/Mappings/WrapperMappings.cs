using System.Security.Cryptography;
using AutoMapper;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.DTO.Extensions;
using Czeum.DTO.Wrappers;

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