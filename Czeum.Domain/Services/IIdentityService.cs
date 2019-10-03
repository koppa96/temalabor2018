using System;

namespace Czeum.Domain.Services
{
    public interface IIdentityService
    {
        string GetCurrentUserName();
        Guid GetCurrentUserId();
    }
}