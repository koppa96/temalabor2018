﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Czeum.DAL.Entities
{
    public class Match
    {
        [Key]
        public int MatchId { get; set; }

        public ApplicationUser Player1 { get; set; }
        public ApplicationUser Player2 { get; set; }

        public MatchState State { get; set; }
        public SerializedBoard Board { get; set; }
    }
}
