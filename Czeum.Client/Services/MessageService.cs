using Czeum.Client.Interfaces;
using Czeum.Core.DTOs;
using Czeum.Core.Services;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Client.Services
{
    class MessageService : IMessageService
    {
        private string BASE_URL = App.Current.Resources["BaseUrl"].ToString();

        private IUserManagerService userManagerService;

        public MessageService(IUserManagerService userManagerService)
        {
            this.userManagerService = userManagerService;
        }

        public Task<Message> SendToLobbyAsync(Guid lobbyId, string message)
        {
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment($"api/messages/lobby/{lobbyId}").PostJsonAsync($"\"{message}\"").ReceiveJson<Message>();
        }

        public Task<IEnumerable<Message>> GetMessagesOfLobbyAsync(Guid lobbyId)
        {
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment($"api/messages/lobby/{lobbyId}").GetJsonAsync<IEnumerable<Message>>();
        }

        public Task<Message> SendToMatchAsync(Guid matchId, string message)
        {
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment($"api/messages/match/{matchId}").PostJsonAsync($"\"{message}\"").ReceiveJson<Message>();
        }

        public Task<IEnumerable<Message>> GetMessagesOfMatchAsync(Guid matchId)
        {
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment($"api/messages/match/{matchId}").GetJsonAsync<IEnumerable<Message>>();
        }
    }
}
