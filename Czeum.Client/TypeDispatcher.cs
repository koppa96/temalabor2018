using System.Collections;
using System.Collections.Generic;
using Czeum.Client.Interfaces;

namespace Czeum.Client
{
    public class TypeDispatcher : ITypeDispatcher
    {
        private IEnumerable renderers;

        public TypeDispatcher(IEnumerable<ILobbyRenderer> renderers)
        {
            this.renderers = renderers;
        }
    }
}