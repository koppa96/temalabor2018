using AutoMapper;
using Czeum.Core.DTOs.Abstractions.Lobbies;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.GameServices.ServiceMappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Application.Mappings
{
    public class LobbyGameTypeValueResolver : IValueResolver<LobbyData, LobbyDataWrapper, int>
    {
        public int Resolve(LobbyData source, LobbyDataWrapper destination, int destMember, ResolutionContext context)
        {
            return GameTypeMapping.Instance.GetDisplayDataBy(x => x.LobbyDataType, source.GetType()).Identifier;
        }
    }
}
