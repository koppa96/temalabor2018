using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Client {
    class LobbyRendererAttribute : Attribute {
        public string Identifier { get; }

        public LobbyRendererAttribute(string identifier)
        {
            this.Identifier = identifier;
        }
    }
}
