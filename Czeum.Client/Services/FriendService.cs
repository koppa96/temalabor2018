using Czeum.Client.Interfaces;
using Czeum.Core.DTOs.UserManagement;
using Czeum.Core.Services;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Client.Services
{
    public class FriendService : IFriendService
    {
        private string BASE_URL = App.Current.Resources["BaseUrl"].ToString();

        private IUserManagerService userManagerService;

        public FriendService(IUserManagerService userManagerService)
        {
            this.userManagerService = userManagerService;
        }

        public Task<FriendRequestDto> AddRequestAsync(Guid receiverId)
        {
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment($"api/friends/friend-requests/{receiverId}").PostAsync(null).ReceiveJson<FriendRequestDto>();
        }

        public Task<IEnumerable<FriendDto>> GetFriendsOfUserAsync(string user)
        {
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment($"api/friends/friendships").GetJsonAsync<IEnumerable<FriendDto>>();
        }

        public Task<IEnumerable<FriendRequestDto>> GetRequestsReceivedAsync()
        {
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment($"api/friends/friend-requests/received").GetJsonAsync<IEnumerable<FriendRequestDto>>();
        }

        public Task<IEnumerable<FriendRequestDto>> GetRequestsSentAsync()
        {
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment($"api/friends/friend-requests/sent").GetJsonAsync<IEnumerable<FriendRequestDto>>();
        }

        public Task<FriendDto> AcceptRequestAsync(Guid requestId)
        {
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment($"api/friends/friendships/{requestId}").PostJsonAsync(null).ReceiveJson<FriendDto>();
        }

        public Task RejectRequestAsync(Guid requestId)
        {
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment($"api/friends/friend-requests/{requestId}/reject").DeleteAsync();
        }

        public Task RemoveFriendAsync(Guid friendshipId)
        {
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment($"api/friends/friendships/{friendshipId}").DeleteAsync();
        }

        public Task RevokeRequestAsync(Guid requestId)
        {
            return BASE_URL.WithOAuthBearerToken(userManagerService.AccessToken).AppendPathSegment($"api/friends/friend-requests/{requestId}/cancel").DeleteAsync();
        }
    }
}
