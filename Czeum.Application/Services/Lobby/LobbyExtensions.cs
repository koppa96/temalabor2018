using System;
using System.Collections.Generic;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;

namespace Czeum.Application.Services.Lobby
{
    public static class LobbyExtensions
    {
        public static void JoinGuest(this LobbyData lobby, string player, List<string> friends)
        {
            if (lobby.Guest != null)
            {
                throw new InvalidOperationException("The lobby is full.");
            }

            if (lobby.Access == LobbyAccess.Private && !lobby.InvitedPlayers.Contains(player) ||
                lobby.Access == LobbyAccess.FriendsOnly && !friends.Contains(player))
            {
                throw new UnauthorizedAccessException("The user is not authorized to join the lobby.");
            }
            
            lobby.InvitedPlayers.Remove(player);
            lobby.Guest = player;
        }

        public static void DisconnectPlayer(this LobbyData lobby, string player)
        {
            if (lobby.Guest == player)
            {
                lobby.Guest = null;
            }
            else if (lobby.Host == player)
            {
                lobby.Host = lobby.Guest;
                lobby.Guest = null;
            }
        }
    }
}