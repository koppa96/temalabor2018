using System;
using Czeum.Core.DTOs.Abstractions;
using Czeum.Core.DTOs.Extensions;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Czeum.Core.DTOs.Converters
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