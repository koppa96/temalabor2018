using Czeum.Core.DTOs.Abstractions;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.GameServices.ServiceMappings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

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
            var gameIdentifier = obj.GetValue("GameType", StringComparison.OrdinalIgnoreCase).Value<int>();
            var moveType = GameTypeMapping.Instance.GetMoveResultType(gameIdentifier);
            return new MoveResultWrapper
            {
                GameIdentifier = gameIdentifier,
                Content = JsonConvert.DeserializeObject(obj.GetValue("Content", StringComparison.OrdinalIgnoreCase).ToString(), moveType) as IMoveResult
            };
        }
    }
}