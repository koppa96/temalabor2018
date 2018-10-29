using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4Server.Hubs {
    [Authorize]
    public class GameHub : Hub {
        public string Hello(string name) {
            return "Hello " + name;
        }
    }
}
