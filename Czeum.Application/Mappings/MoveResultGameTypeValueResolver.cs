using AutoMapper;
using Czeum.Core.DTOs.Abstractions;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.GameServices.ServiceMappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Application.Mappings
{
    public class MoveResultGameTypeValueResolver : IValueResolver<IMoveResult, MoveResultWrapper, int>
    {
        public int Resolve(IMoveResult source, MoveResultWrapper destination, int destMember, ResolutionContext context)
        {
            return GameTypeMapping.Instance.GetDisplayDataBy(x => x.MoveResultType, source.GetType()).Identifier;
        }
    }
}
