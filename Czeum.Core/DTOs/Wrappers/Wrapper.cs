using Czeum.Core.Enums;

namespace Czeum.Core.DTOs.Wrappers
{
    /// <summary>
    /// Wraps an object together with a GameType discriminator
    /// </summary>
    /// <typeparam name="TContent">The type of the content</typeparam>
    public class Wrapper<TContent>
    {
        public int GameIdentifier { get; set; }
        public TContent Content { get; set; }
    }
}