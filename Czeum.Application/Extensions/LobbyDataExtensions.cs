using Czeum.Abstractions.DTO.Lobbies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.Application.Extensions
{
    public static class LobbyDataExtensions
    {
        public static bool Contains(this LobbyData lobbyData, string username)
        {
            return lobbyData.Host == username || lobbyData.Guests.Contains(username);
        }
    }
}
