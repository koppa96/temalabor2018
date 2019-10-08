using System;

namespace Czeum.Core.Domain
{
    public abstract class SerializedBoard : EntityBase
    {
        public string BoardData { get; set; }
        public Guid MatchId { get; set; }
    }
}
