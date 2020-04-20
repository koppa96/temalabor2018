using Czeum.Core.DTOs.Abstractions.Lobbies;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.GameServices.ServiceMappings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Czeum.Core.DTOs.Converters
{
    public class LobbyDataWrapperConverter : JsonConverter<LobbyDataWrapper>
    {
        private readonly GameTypeMapping gameTypeMapping;

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, LobbyDataWrapper value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override LobbyDataWrapper ReadJson(JsonReader reader, Type objectType, LobbyDataWrapper existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            var GameIdentifier = obj.GetValue("GameIdentifier", StringComparison.OrdinalIgnoreCase).Value<int>();
            var lobbyType = GameTypeMapping.Instance.GetLobbyDataType(GameIdentifier);
            return new LobbyDataWrapper
            {
                GameIdentifier = GameIdentifier,
                Content = JsonConvert.DeserializeObject(obj.GetValue("Content", StringComparison.OrdinalIgnoreCase).ToString(), lobbyType) as LobbyData
            };
        }
    }
}