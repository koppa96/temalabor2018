using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.DTO;
using Microsoft.Extensions.Logging;

namespace Czeum.Server.Hubs
{
    public partial class GameHub
    {
        public async Task<LobbyData> CreateLobby(Type lobbyType)
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

            try
            {
                var lobby = _lobbyService.CreateLobby(lobbyType);
                
                lobby.Host = Context.UserIdentifier;
                lobby.Guest = null;
                lobby.InvitedPlayers = new List<string>();
                _lobbyService.AddLobby(lobby);
                _logger.LogInformation($"Lobby created by {Context.UserIdentifier}, Id: {lobby.LobbyId}");
            
                await Clients.All.LobbyCreated(lobby);
                return lobby;
            }
            catch (ArgumentException e)
            {
                await Clients.Caller.ReceiveError(ErrorCodes.InvalidLobbyType);
                return null;
            }
        }

        public async Task UpdateLobby(LobbyData lobbyData)
        {
            if (!await this.LobbyValidationCallbacks(_lobbyService, lobbyData.LobbyId))
            {
                return;
            }
            
            var result = lobbyData.ValidateSettings();
            if (result != null)
            {
                await Clients.Caller.ReceiveError(result);
                return;
            }
            
            _lobbyService.UpdateLobbySettings(lobbyData);
            await Clients.All.LobbyChanged(lobbyData);
        }

        public async Task InvitePlayer(int lobbyId, string player)
        {
            if (!await this.LobbyValidationCallbacks(_lobbyService, lobbyId))
            {
                return;
            }

            _lobbyService.InvitePlayerToLobby(lobbyId, player);
            await Clients.All.LobbyChanged(_lobbyService.GetLobby(lobbyId));
        }

        public async Task CancelInvitation(int lobbyId, string player)
        {
            if (!await this.LobbyValidationCallbacks(_lobbyService, lobbyId))
            {
                return;
            }
            
            _lobbyService.CancelInviteFromLobby(lobbyId, player);
            await Clients.All.LobbyChanged(_lobbyService.GetLobby(lobbyId));
        }

        public async Task JoinLobby(int lobbyId)
        {
            if (!await this.LobbyValidationCallbacks(_lobbyService, lobbyId))
            {
                return;
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
            if (!await this.LobbyValidationCallbacks(_lobbyService, lobbyId))
            {
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