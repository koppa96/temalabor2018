using System;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.DTO.Extensions;
using Czeum.DTO.Lobbies;
using Czeum.DTO.Wrappers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Czeum.DTO.Converters
{
    public class MoveDataWrapperConverter : JsonConverter<MoveDataWrapper>
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, MoveDataWrapper value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override MoveDataWrapper ReadJson(JsonReader reader, Type objectType, MoveDataWrapper existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            var gameType = (GameType)obj.GetValue("GameType", StringComparison.OrdinalIgnoreCase).Value<int>();
            var moveType = gameType.GetMoveDataType();
            return new MoveDataWrapper
            {
                GameType = gameType,
                Content = JsonConvert.DeserializeObject(obj.GetValue("Content", StringComparison.OrdinalIgnoreCase).ToString(), moveType) as MoveData
            };
        }
    }
}