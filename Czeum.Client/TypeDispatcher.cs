using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Czeum.Abstractions.DTO;
using Czeum.Client.Interfaces;

namespace Czeum.Client
{
    public class TypeDispatcher : ITypeDispatcher
    {
        private IEnumerable<ILobbyRenderer> renderers;

        public TypeDispatcher(IEnumerable<ILobbyRenderer> renderers)
        {
            this.renderers = renderers;
        }

        public ILobbyRenderer DispatchLobbyRenderer(LobbyData lobbyData)
        {
            var renderer = renderers.FirstOrDefault(r => Attribute.GetCustomAttributes(r.GetType())
                .Any(a => a is LobbyRendererAttribute attr && attr.LobbyType == lobbyData.GetType()));
            return renderer;
        }
    }
}