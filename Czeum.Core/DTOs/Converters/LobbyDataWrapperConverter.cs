using System;
using Czeum.Core.DTOs.Abstractions.Lobbies;
using Czeum.Core.DTOs.Extensions;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Czeum.Core.DTOs.Converters
{
    public class LobbyDataWrapperConverter : JsonConverter<LobbyDataWrapper>
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, LobbyDataWrapper value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override LobbyDataWrapper ReadJson(JsonReader reader, Type objectType, LobbyDataWrapper existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            var gameType = (GameType)obj.GetValue("GameType", StringComparison.OrdinalIgnoreCase).Value<int>();
            var lobbyType = gameType.GetLobbyType();
            return new LobbyDataWrapper
            {
                GameType = gameType,
                Content = JsonConvert.DeserializeObject(obj.GetValue("Content", StringComparison.OrdinalIgnoreCase).ToString(), lobbyType) as LobbyData
            };
        }
    }
}