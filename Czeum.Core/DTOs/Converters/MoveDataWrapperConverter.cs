using Czeum.Core.DTOs.Abstractions;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.GameServices.ServiceMappings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

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
            var gameIdentifier = obj.GetValue("GameIdentifier", StringComparison.OrdinalIgnoreCase).Value<int>();
            var moveType = GameTypeMapping.Instance.GetMoveDataType(gameIdentifier);
            return new MoveDataWrapper
            {
                GameIdentifier = gameIdentifier,
                Content = JsonConvert.DeserializeObject(obj.GetValue("Content", StringComparison.OrdinalIgnoreCase).ToString(), moveType) as MoveData
            };
        }
    }
}