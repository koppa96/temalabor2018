using Czeum.Abstractions;
using Czeum.Abstractions.Domain;
using System;

namespace Czeum.Domain.Entities
{
    public abstract class SerializedBoard : EntityBase, ISerializedBoard
    {
        public string BoardData { get; set; }

        public Match Match { get; set; }
        public Guid MatchId { get; set; }
    }
}
