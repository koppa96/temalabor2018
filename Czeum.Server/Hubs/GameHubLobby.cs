using System;
using System.Threading.Tasks;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.DTO;
using Microsoft.Extensions.Logging;

namespace Czeum.Server.Hubs
{
    public partial class GameHub
    {
        public async Task CreateLobby(Type lobbyType, LobbyAccess access, string name)
        {
            if (!await this.LobbyJoinValidationCallbacks(_lobbyService, _soloQueueService))
            {
                return;
            }

            try
            {
                var lobby = _lobbyService.CreateAndAddLobby(lobbyType, Context.UserIdentifier, access, name);
                _logger.LogInformation($"Lobby created by {Context.UserIdentifier}, Id: {lobby.LobbyId}");
            
                await Clients.Caller.LobbyCreated(lobby);
                await Clients.Others.LobbyAdded(lobby);
            }
            catch (ArgumentException)
            {
                await Clients.Caller.ReceiveError(ErrorCodes.InvalidLobbyType);
                return;
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
            if (!_lobbyService.LobbyExists(lobbyId))
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NoSuchLobby);
                return;
            }

            if (!await this.LobbyJoinValidationCallbacks(_lobbyService, _soloQueueService))
            {
                return;
            }

            if (await _lobbyService.JoinPlayerToLobbyAsync(Context.UserIdentifier, lobbyId))
            {
                var lobby = _lobbyService.GetLobby(lobbyId);
                await Clients.Caller.JoinedToLobby(lobby, _messageService.GetMessagesOfLobby(lobbyId));
                await Clients.All.LobbyChanged(lobby);
                return;
            }

            await Clients.Caller.ReceiveError(ErrorCodes.CouldNotJoinLobby);
        }

        public async Task DisconnectFromLobby(int lobbyId)
        {
            if (!_lobbyService.LobbyExists(lobbyId))
            {
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

        public async Task CreateMatch(int lobbyId)
        {
            if (!await this.LobbyValidationCallbacks(_lobbyService, lobbyId))
            {
                return;
            }

            var lobby = _lobbyService.GetLobby(lobbyId);
            if (lobby.Guest == null)
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NotEnoughPlayers);
                return;
            }

            try
            {
                var statuses = await _gameHandler.CreateMatchAsync(lobby);
                _lobbyService.RemoveLobby(lobbyId);

                await Clients.Caller.MatchCreated(statuses[lobby.Host]);
                await Clients.User(lobby.Guest).MatchCreated(statuses[lobby.Guest]);
                await Clients.All.LobbyDeleted(lobbyId);
            }
            catch (GameNotSupportedException)
            {
                await Clients.Caller.ReceiveError(ErrorCodes.GameNotSupported);
            }
        }

        public async Task SendMessageToLobby(int lobbyId, string message)
        {
            if (!_lobbyService.LobbyExists(lobbyId))
            {
                await Clients.Caller.ReceiveError(ErrorCodes.NoSuchLobby);
                return;
            }

            var msg = _messageService.SendToLobby(lobbyId, message, Context.UserIdentifier);
            if (msg == null)
            {
                await Clients.Caller.ReceiveError(ErrorCodes.CannotSendMessage);
                return;
            }
            
            await Clients.Caller.LobbyMessageSent(msg);
            await Clients.User(_lobbyService.GetOtherPlayer(lobbyId, Context.UserIdentifier)).ReceiveLobbyMessage(msg);
        }
    }
}