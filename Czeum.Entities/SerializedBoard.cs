using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Czeum.Entities
{
    public abstract class SerializedBoard
    {
        [Key]
        public int BoardId { get; set; }
        public string BoardData { get; set; }

        public Match Match { get; set; }

        [ForeignKey("Match")]
        public int MatchId { get; set; }
    }
}
