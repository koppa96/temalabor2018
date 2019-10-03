using System;
using Czeum.Abstractions.Domain;

namespace Czeum.Domain.Entities.Boards
{
    public abstract class SerializedBoard : EntityBase, ISerializedBoard
    {
        public string BoardData { get; set; }

        public Match Match { get; set; }
        public Guid MatchId { get; set; }
    }
}
