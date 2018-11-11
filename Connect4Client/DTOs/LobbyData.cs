using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Client.DTOs {
    class LobbyData {
        private List<String> invitedPlayers = new List<string>();

        public int Id { get; set; }
        public String Leader { get; set; }
        public bool Open { get; set; }
        public List<String> InvitedPlayers { get { return invitedPlayers; } set { invitedPlayers = value; } }
    }
}
