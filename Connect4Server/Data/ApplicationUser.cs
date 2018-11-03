using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4Server.Data {
    public class ApplicationUser : IdentityUser {
        [InverseProperty("Player1")]
        public List<Match> Player1Matches { get; set; }

        [InverseProperty("Player2")]
        public List<Match> Player2Matches { get; set; }
	}
}
