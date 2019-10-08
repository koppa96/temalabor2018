using Czeum.Core.DTOs.Abstractions;
using Czeum.Core.DTOs.Converters;
using Newtonsoft.Json;

namespace Czeum.Core.DTOs.Wrappers
{
    [JsonConverter(typeof(MoveDataWrapperConverter))]
    public class MoveDataWrapper : Wrapper<MoveData>
    {
    }
}