using System.Collections.Generic;
using Czeum.Abstractions.DTO;
using Microsoft.Extensions.Hosting;

namespace Czeum.Server.Services.Lobby
{
    public static class LobbyExtensions
    {
        public static bool JoinGuest(this LobbyData lobby, string player, List<string> friends)
        {
            if (lobby.Guest == null && (lobby.Access == LobbyAccess.Public ||
                                        lobby.InvitedPlayers.Contains(player) ||
                                        lobby.Access == LobbyAccess.FriendsOnly && friends.Contains(player)))
            {
                lobby.InvitedPlayers.Remove(player);
                lobby.Guest = player;
                return true;
            }

            return false;
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