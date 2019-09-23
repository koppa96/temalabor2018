using Czeum.DTO.Lobbies;

namespace Czeum.DTO.Wrappers
{
    public class Wrapper<TContent>
    {
        public GameType GameType { get; set; }
        public TContent Content { get; set; }
    }
}