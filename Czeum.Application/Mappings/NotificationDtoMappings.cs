using AutoMapper;
using Czeum.Core.DTOs.Notifications;
using Czeum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Application.Mappings
{
    public class NotificationDtoMappings : Profile
    {
        public NotificationDtoMappings()
        {
            CreateMap<Notification, NotificationDto>()
                .ForMember(x => x.SenderUserName, o => o.MapFrom(x => x.SenderUser.UserName));
        }
    }
}
