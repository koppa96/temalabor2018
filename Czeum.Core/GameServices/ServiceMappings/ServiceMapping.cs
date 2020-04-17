using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Core.GameServices.ServiceMappings
{
    public class ServiceMapping
    {
        public string DisplayName { get; }
        public Type LobbyDataType { get; }
        public Type MoveDataType { get; }
        public Type MoveResultType { get; }

        public ServiceMapping(string displayName, Type lobbyDataType, Type moveDataType, Type moveResultType)
        {
            DisplayName = displayName;
            LobbyDataType = lobbyDataType;
            MoveDataType = moveDataType;
            MoveResultType = moveResultType;
        }
    }
}
