using AutoMapper;
using Czeum.Core.DTOs.Achivement;
using Czeum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Application.Mappings
{
    public class AchivementMappings : Profile
    {
        public AchivementMappings()
        {
            CreateMap<UserAchivement, AchivementDto>()
                .ForMember(x => x.Title, o => o.MapFrom(x => x.Achivement.Title))
                .ForMember(x => x.Description, o => o.MapFrom(x => x.Achivement.Description));
        }
    }
}
