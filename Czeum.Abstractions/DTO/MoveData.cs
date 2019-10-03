using System;

namespace Czeum.Abstractions.DTO
{
    /// <summary>
    /// An abstract base class for moves.
    /// </summary>
    public abstract class MoveData
    {
        public Guid MatchId { get; set; }
    }
}
