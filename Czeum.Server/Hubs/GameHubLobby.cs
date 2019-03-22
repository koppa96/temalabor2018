using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.DTO;
using Microsoft.Extensions.Logging;

namespace Czeum.Server.Hubs
{
    public partial class GameHub
    {
        public async Task<LobbyData> CreateLobby(LobbyData lobbyData)
        {
            if (_lobbyService.FindUserLobby(Context.UserIdentifier) != null)
            {
                await Clients.Caller.ReceiveError(ErrorCodes.AlreadyInLobby);
                return null;
            }

            if (_soloQueueService.IsQueuing(Context.UserIdentifier))
            {
                await Clients.Caller.ReceiveError(ErrorCodes.AlreadyQueuing);
                return null;
            }
            
            var lobbyWithId = _lobbyService.AddLobby(lobbyData);
            _logger.LogInformation($"Lobby created by {Context.UserIdentifier}, Id: {lobbyWithId.LobbyId}");
            
            await Clients.All.LobbyCreated(lobbyWithId);
            return lobbyWithId;
        }

        public async Task UpdateLobby(LobbyData lobbyData)
        {
            if (!_lobbyService.ValidateModifier(Context.UserIdentifier, lobbyData.LobbyId))
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NoRightToChange);
            }
            
            var result = lobbyData.ValidateSettings();
            if (result != null)
            {
                await Clients.Caller.ReceiveError(result);
                return;
            }
            
            _lobbyService.UpdateLobbySettings(ref lobbyData);
            await Clients.All.LobbyChanged(lobbyData);
        }

        public async Task InvitePlayer(int lobbyId, string player)
        {
            if (!_lobbyService.LobbyExists(lobbyId))
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NoSuchLobby);
                return;
            }
            
            if (!_lobbyService.ValidateModifier(Context.UserIdentifier, lobbyId))
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NoRightToChange);
                return;
            }

            _lobbyService.InvitePlayerToLobby(lobbyId, player);
            await Clients.All.LobbyChanged(_lobbyService.GetLobby(lobbyId));
        }

        public async Task CancelInvitation(int lobbyId, string player)
        {
            if (!_lobbyService.LobbyExists(lobbyId))
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NoSuchLobby);
                return;
            }
            
            if (!_lobbyService.ValidateModifier(Context.UserIdentifier, lobbyId))
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NoRightToChange);
                return;
            }
            
            _lobbyService.CancelInviteFromLobby(lobbyId, player);
            await Clients.All.LobbyChanged(_lobbyService.GetLobby(lobbyId));
        }

        public async Task JoinLobby(int lobbyId)
        {
            if (!_lobbyService.LobbyExists(lobbyId))
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NoSuchLobby);
                return;
            }

            if (!_lobbyService.JoinPlayerToLobby(Context.UserIdentifier, lobbyId))
            {
                await Clients.Caller.ReceiveError(ErrorCodes.CouldNotJoinLobby);
            }
            
            var lobby = _lobbyService.GetLobby(lobbyId);
            await Clients.Caller.JoinedToLobby(lobby);
            await Clients.All.LobbyChanged(lobby);
        }

        public async Task DisconnectFromLobby(int lobbyId)
        {
            if (!_lobbyService.LobbyExists(lobbyId))
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NoSuchLobby);
                return;
            }
            
            _lobbyService.DisconnectPlayerFromLobby(Context.UserIdentifier, lobbyId);
            if (_lobbyService.LobbyExists(lobbyId))
            {
                await Clients.All.LobbyChanged(_lobbyService.GetLobby(lobbyId));
            }
            else
            {
                await Clients.All.LobbyDeleted(lobbyId);
            }
        }

        public async Task KickGuest(int lobbyId)
        {
            if (!_lobbyService.LobbyExists(lobbyId))
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NoSuchLobby);
                return;
            }
            
            if (!_lobbyService.ValidateModifier(Context.UserIdentifier, lobbyId))
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NoRightToChange);
                return;
            }

            var kickedGuest = _lobbyService.KickGuest(lobbyId);
            if (kickedGuest != null)
            {
                await Clients.User(kickedGuest).KickedFromLobby();
                await Clients.All.LobbyChanged(_lobbyService.GetLobby(lobbyId));
            }
        }
    }
}