using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Api.Common;
using Czeum.Api.SignalR;
using Czeum.Application.Services.Lobby;
using Czeum.Application.Services.MessageService;
using Czeum.ClientCallback;
using Czeum.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Czeum.Api.Controllers.Messages
{
    [Route(ApiResources.Messages.Lobby.BasePath)]
    [ApiController]
    public class LobbyMessagesController : ControllerBase
    {
        private readonly IMessageService messageService;
        private readonly IHubContext<NotificationHub, ICzeumClient> hubContext;
        private readonly ILobbyService lobbyService;

        public LobbyMessagesController(IMessageService messageService,
            IHubContext<NotificationHub, ICzeumClient> hubContext,
            ILobbyService lobbyService)
        {
            this.messageService = messageService;
            this.hubContext = hubContext;
            this.lobbyService = lobbyService;
        }

        [HttpGet("{lobbyId}")]
        public ActionResult<IEnumerable<Message>> GetMessages(Guid lobbyId)
        {
            return Ok(messageService.GetMessagesOfLobby(lobbyId));
        }

        [HttpPost("{lobbyId}")]
        public async Task<ActionResult<Message>> SendMessage(Guid lobbyId, [FromBody] string message)
        {
            var sentMessage = messageService.SendToLobby(lobbyId, message);

            await hubContext.Clients.Users(lobbyService.GetOthersInLobby(lobbyId).ToList())
                .ReceiveLobbyMessage(sentMessage);
            
            return Ok(sentMessage);
        }
    }
}