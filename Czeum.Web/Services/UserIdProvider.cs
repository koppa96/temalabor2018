using System;
using Microsoft.AspNetCore.SignalR;

namespace Czeum.Api.Services
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.Identity.Name ?? throw new InvalidOperationException("Could not determine user.");
        }
    }
}