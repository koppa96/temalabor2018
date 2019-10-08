using Czeum.Core.DTOs.Abstractions.Lobbies;
using Czeum.Core.DTOs.Converters;
using Newtonsoft.Json;

namespace Czeum.Core.DTOs.Wrappers
{
    [JsonConverter(typeof(LobbyDataWrapperConverter))]
    public class LobbyDataWrapper : Wrapper<LobbyData>
    {
    }
}