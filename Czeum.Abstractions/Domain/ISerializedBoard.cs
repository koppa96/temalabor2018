using Czeum.Abstractions.DTO;

namespace Czeum.Abstractions.Domain
{
    /// <summary>
    /// Represents a board stored in the database.
    /// </summary>
    public interface ISerializedBoard
    {
        string BoardData { get; set; }
    }
}