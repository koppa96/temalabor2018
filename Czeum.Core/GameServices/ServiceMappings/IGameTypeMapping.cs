using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Core.GameServices.ServiceMappings
{
    public interface IGameTypeMapping
    {
        Type GetLobbyDataType(int gameIdentifier);
        Type GetMoveDataType(int gameIdentifier);
        Type GetMoveResultType(int gameIdentifier);
        IEnumerable<(int Identifier, string DisplayName)> GetGameTypeNames();
        (int Identifier, string DisplayName) GetDisplayDataBy<TProperty>(Func<ServiceMapping, TProperty> selector, TProperty value);
    }
}
