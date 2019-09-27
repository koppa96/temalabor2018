using AutoMapper;
using Czeum.Domain.Entities;
using Czeum.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Application.Mappings
{
    public class MessageMappings : Profile
    {
        public MessageMappings()
        {
            CreateMap<StoredMessage, Message>()
                .ForMember(dst => dst.Sender, cfg => cfg.MapFrom(src => src.Sender.UserName));
        }
    }
}
