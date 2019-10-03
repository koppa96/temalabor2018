using Czeum.Abstractions.DTO.Lobbies;
using Czeum.DTO.Converters;
using Newtonsoft.Json;

namespace Czeum.DTO.Wrappers
{
    [JsonConverter(typeof(LobbyDataWrapperConverter))]
    public class LobbyDataWrapper : Wrapper<LobbyData>
    {
    }
}