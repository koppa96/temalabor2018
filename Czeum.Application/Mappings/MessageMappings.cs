using AutoMapper;
using Czeum.Core.DTOs;
using Czeum.Domain.Entities;

namespace Czeum.Application.Mappings
{
    public class MessageMappings : Profile
    {
        public MessageMappings()
        {
            CreateMap<StoredMessage, Message>()
                .ForMember(dst => dst.Sender, cfg => cfg.MapFrom(src => src.Sender.UserName));

            CreateMap<DirectMessage, Message>()
                .ForMember(dst => dst.Sender, cfg => cfg.MapFrom(src => src.Sender.UserName));
        }
    }
}
