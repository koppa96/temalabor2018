using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Client {
    class LobbyRendererAttribute : Attribute {
        public Type LobbyType { get; }

        public LobbyRendererAttribute(Type lobbyType)
        {
            this.LobbyType = lobbyType;
        }
    }
}
