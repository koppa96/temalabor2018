using System;

namespace Czeum.Core.DTOs.Abstractions
{
    /// <summary>
    /// An abstract base class for moves.
    /// </summary>
    public abstract class MoveData
    {
        public Guid MatchId { get; set; }
    }
}
