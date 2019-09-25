using Czeum.DTO.Lobbies;

namespace Czeum.DTO.Wrappers
{
    /// <summary>
    /// Wraps an object together with a GameType discriminator
    /// </summary>
    /// <typeparam name="TContent">The type of the content</typeparam>
    public class Wrapper<TContent>
    {
        public GameType GameType { get; set; }
        public TContent Content { get; set; }
    }
}