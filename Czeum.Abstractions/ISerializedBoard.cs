using Czeum.Abstractions.DTO;

namespace Czeum.Abstractions
{
    /// <summary>
    /// Represents a board stored in the database.
    /// </summary>
    public interface ISerializedBoard
    {
        string BoardData { get; set; }
    }
}