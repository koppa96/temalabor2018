using System;
using System.Collections.Generic;
using System.Linq;
using Czeum.Abstractions.DTO.Lobbies;

namespace Czeum.Application.Extensions
{
    public static class LobbyExtensions
    {
        public static void JoinGuest(this LobbyData lobby, string player, List<string> friends)
        {
            if (lobby.Guests.Count == lobby.MaximumPlayerCount - 1)
            {
                throw new InvalidOperationException("The lobby is full.");
            }

            if (lobby.Access == LobbyAccess.Private && !lobby.InvitedPlayers.Contains(player) ||
                lobby.Access == LobbyAccess.FriendsOnly && !friends.Contains(player) && !lobby.InvitedPlayers.Contains(player))
            {
                throw new UnauthorizedAccessException("The user is not authorized to join the lobby.");
            }
            
            lobby.InvitedPlayers.Remove(player);
            lobby.Guests.Add(player);
            lobby.LastModified = DateTime.UtcNow;
        }

        public static void DisconnectPlayer(this LobbyData lobby, string player)
        {
            if (lobby.Host == player)
            {
                lobby.Host = null;
                if (lobby.Guests.Count > 0)
                {
                    lobby.Host = lobby.Guests.First();
                }
            }
            else
            {
                lobby.Guests.Remove(player);
            }
        }

        public static IEnumerable<string> Others(this LobbyData lobby, string player)
        {
            return lobby.Guests.Append(lobby.Host).Where(p => p != player);
        }
        
        public static bool Contains(this LobbyData lobbyData, string username)
        {
            return lobbyData.Host == username || lobbyData.Guests.Contains(username);
        }
    }
}