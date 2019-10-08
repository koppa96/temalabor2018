namespace Czeum.Core.Services
{
    /// <summary>
    /// Interface for Solo Queue services.
    /// </summary>
    public interface ISoloQueueService
    {
        /// <summary>
        /// Adds a player to the solo queue.
        /// </summary>
        /// <param name="user">The name of the player</param>
        void JoinSoloQueue(string user);

        /// <summary>
        /// Removes a player from the solo queue.
        /// </summary>
        /// <param name="user">The name of the player</param>
        void LeaveSoloQueue(string user);

        /// <summary>
        /// Gets the first 2 players from the list and removes them. If there are less then 2 players it returns null.
        /// </summary>
        /// <returns>The names of the first 2 players</returns>
        string[] PopFirstTwoPlayers();

        /// <summary>
        /// Determines whether the user with the given name is queuing.
        /// </summary>
        /// <param name="user">The name of the user</param>
        /// <returns>Whether the user is queuing</returns>
        bool IsQueuing(string user);
    }
}