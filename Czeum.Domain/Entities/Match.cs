using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Czeum.Domain.Entities
{
    public class Match : EntityBase
    {
        public ApplicationUser Player1 { get; set; }
        public ApplicationUser Player2 { get; set; }

        public MatchState State { get; set; }
        public SerializedBoard Board { get; set; }

        public List<StoredMessage> Messages { get; set; }
    }
}
