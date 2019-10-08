using Czeum.Core.DTOs.Abstractions;
using Czeum.Core.DTOs.Converters;
using Newtonsoft.Json;

namespace Czeum.Core.DTOs.Wrappers
{
    [JsonConverter(typeof(MoveResultWrapperConverter))]
    public class MoveResultWrapper : Wrapper<IMoveResult>
    {
    }
}