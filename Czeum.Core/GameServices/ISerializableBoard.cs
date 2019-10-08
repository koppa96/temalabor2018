namespace Czeum.Core.GameServices
{
    /// <summary>
    /// Represents a board stored in the business logic that can be serialized.
    /// </summary>
    /// <typeparam name="T">The serialized type of the board</typeparam>
    public interface ISerializableBoard<T>
    {
        /// <summary>
        /// Serializes tha board into its serialized representation.
        /// </summary>
        /// <returns>The serialized board</returns>
        T SerializeContent();

        /// <summary>
        /// Fills the board from its serialized representation.
        /// </summary>
        /// <param name="serializedBoard">The serialized board</param>
        void DeserializeContent(T serializedBoard);
    }
}
