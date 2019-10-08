using System;
using Czeum.Core.DTOs.Abstractions;
using Czeum.Core.DTOs.Extensions;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Czeum.Core.DTOs.Converters
{
    public class MoveResultWrapperConverter : JsonConverter<MoveResultWrapper>
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, MoveResultWrapper value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override MoveResultWrapper ReadJson(JsonReader reader, Type objectType, MoveResultWrapper existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            var gameType = (GameType)obj.GetValue("GameType", StringComparison.OrdinalIgnoreCase).Value<int>();
            var moveType = gameType.GetMoveResultType();
            return new MoveResultWrapper
            {
                GameType = gameType,
                Content = JsonConvert.DeserializeObject(obj.GetValue("Content", StringComparison.OrdinalIgnoreCase).ToString(), moveType) as IMoveResult
            };
        }
    }
}