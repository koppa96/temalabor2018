using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4Server.Data {
    public class Match {
        [Key]
        public int MatchId { get; set; }

        public ApplicationUser Player1 { get; set; }
        public ApplicationUser Player2 { get; set; }

        public string BoardData { get; set; }
        public string State { get; set; }
    }
}
