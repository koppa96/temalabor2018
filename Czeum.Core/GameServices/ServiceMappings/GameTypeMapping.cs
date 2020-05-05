using Czeum.Core.DTOs.Abstractions;
using Czeum.Core.DTOs.Abstractions.Lobbies;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Czeum.Core.GameServices.ServiceMappings
{
    public class GameTypeMapping : IGameTypeMapping
    {
        private List<ServiceMapping> serviceMappings = new List<ServiceMapping>();

        public static GameTypeMapping Instance { get; } = new GameTypeMapping();

        public void RegisterServiceMapping<TLobbyData, TMoveData, TMoveResult>(string displayName)
            where TLobbyData : LobbyData
            where TMoveData : MoveData
            where TMoveResult : IMoveResult
        {
            RegisterServiceMapping(displayName, typeof(TLobbyData), typeof(TMoveData), typeof(TMoveResult));
        }

        public void RegisterServiceMapping(string displayName, Type lobbyDataType, Type moveDataType, Type moveResultType)
        {
            serviceMappings.Add(new ServiceMapping(displayName, lobbyDataType, moveDataType, moveResultType));
        }

        public Type GetLobbyDataType(int gameIdentifier)
        {
            return serviceMappings[gameIdentifier].LobbyDataType;
        }

        public Type GetMoveDataType(int gameIdentifier)
        {
            return serviceMappings[gameIdentifier].MoveDataType;
        }

        public Type GetMoveResultType(int gameIdentifier)
        {
            return serviceMappings[gameIdentifier].MoveResultType;
        }

        public IEnumerable<(int Identifier, string DisplayName)> GetGameTypeNames()
        {
            return Enumerable.Range(0, serviceMappings.Count)
                .Select(x => (x, serviceMappings[x].DisplayName))
                .ToList();
        }

        public (int Identifier, string DisplayName) GetDisplayDataBy<TProperty>(
            Func<ServiceMapping, TProperty> selector,
            TProperty value)
        {
            var result = Enumerable.Range(0, serviceMappings.Count)
                .Select(x => new { Identifier = x, Value = serviceMappings[x] })
                .Single(x => selector(x.Value).Equals(value));

            return (result.Identifier, result.Value.DisplayName);
        }

        public void Clear()
        {
            serviceMappings.Clear();
        }
    }
}
